using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CCPromptLauncher
{
    public class PromptInfo
    {
        public string Id;
        public string Title;
        public string FilePath;
    }

    /// <summary>Claude 安装/配置位置检测结果。</summary>
    public class ClaudePathInfo
    {
        public string Label;
        public string Path;
        public bool Exists;
        public bool IsConfig;   // true=配置目录（CLAUDE.md 所在），false=程序位置
    }

    public static class PromptLib
    {
        public const string GenMarker = "项目自动加载规则";
        public static string PromptRoot;
        public static string PromptDir;
        public static string TemplateFile;

        public static void UsePromptLanguage(string code)
        {
            if (string.IsNullOrEmpty(PromptRoot)) PromptRoot = PromptDir;
            string root = PromptRoot;
            string candidate = string.Equals(code, "zh-CN", StringComparison.OrdinalIgnoreCase)
                ? root : Path.Combine(root, code ?? "zh-CN");
            PromptDir = Directory.Exists(candidate) ? candidate : root;
        }

        public static List<PromptInfo> ListPrompts()
        {
            var list = new List<PromptInfo>();
            foreach (string f in Directory.GetFiles(PromptDir, "*.md"))
            {
                string first = "";
                using (var sr = new StreamReader(f, Encoding.UTF8, true))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.Length == 0) continue;
                        if (line.StartsWith("#")) first = line.TrimStart('#', ' ').Trim();
                        break;
                    }
                    if (first.Length == 0) first = Path.GetFileNameWithoutExtension(f);
                }
                list.Add(new PromptInfo { Id = Path.GetFileNameWithoutExtension(f), Title = first, FilePath = f });
            }
            list.Sort((a, b) => string.Compare(a.Id, b.Id, StringComparison.Ordinal));
            return list;
        }

        public static string Resolve(string name)
        {
            string exact = Path.Combine(PromptDir, name + ".md");
            if (File.Exists(exact)) return exact;
            string[] matches = Directory.GetFiles(PromptDir, name + "*.md");
            if (matches.Length > 0) return matches[0];
            throw new FileNotFoundException(AppText.T("PromptNotFound", name));
        }

        /// <summary>用户级配置目录：优先 CLAUDE_CONFIG_DIR 环境变量，否则 %USERPROFILE%\.claude。</summary>
        public static string UserClaudeDir()
        {
            string env = Environment.GetEnvironmentVariable("CLAUDE_CONFIG_DIR");
            if (!string.IsNullOrWhiteSpace(env)) return env;
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(home, ".claude");
        }

        public static string UserClaudeMd()
        {
            return Path.Combine(UserClaudeDir(), "CLAUDE.md");
        }

        /// <summary>检测 Claude 安装位置与配置目录。includeSlow 控制是否执行 npm 慢检测。</summary>
        public static List<ClaudePathInfo> DetectClaudePaths(bool includeSlow)
        {
            var list = new List<ClaudePathInfo>();
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string env = Environment.GetEnvironmentVariable("CLAUDE_CONFIG_DIR");
            if (!string.IsNullOrWhiteSpace(env))
                list.Add(new ClaudePathInfo { Label = AppText.T("EnvConfigLabel"), Path = env, Exists = Directory.Exists(env), IsConfig = true });

            string def = Path.Combine(home, ".claude");
            list.Add(new ClaudePathInfo { Label = AppText.T("UserDefaultLabel"), Path = def, Exists = Directory.Exists(def), IsConfig = true });

            string apClaude = Path.Combine(appdata, "Claude");
            list.Add(new ClaudePathInfo { Label = AppText.T("DesktopConfigLabel"), Path = apClaude, Exists = Directory.Exists(apClaude), IsConfig = true });

            string anClaude = Path.Combine(local, "AnthropicClaude");
            list.Add(new ClaudePathInfo { Label = AppText.T("DesktopDataLabel"), Path = anClaude, Exists = Directory.Exists(anClaude), IsConfig = true });

            string native = Path.Combine(home, ".local", "bin", "claude.exe");
            list.Add(new ClaudePathInfo { Label = AppText.T("NativeCliLabel"), Path = native, Exists = File.Exists(native), IsConfig = false });

            string onPath = FindOnPath("claude.exe");
            if (onPath == null) onPath = FindOnPath("claude.cmd");
            if (onPath == null) onPath = FindOnPath("claude");
            if (onPath != null)
                list.Add(new ClaudePathInfo { Label = AppText.T("PathClaudeLabel"), Path = onPath, Exists = true, IsConfig = false });

            if (includeSlow)
            {
                try
                {
                    string npmRoot = RunQuick("npm.cmd", "root -g");
                    if (!string.IsNullOrEmpty(npmRoot))
                    {
                        string pkg = Path.Combine(npmRoot.Trim(), "@anthropic-ai", "claude-code");
                        list.Add(new ClaudePathInfo { Label = AppText.T("NpmPackageLabel"), Path = pkg, Exists = Directory.Exists(pkg), IsConfig = false });
                    }
                }
                catch { }
            }
            return list;
        }

        /// <summary>生成下一个提示词编号前缀（如 07-）。</summary>
        public static string NextPromptId()
        {
            int max = -1;
            foreach (string f in Directory.GetFiles(PromptDir, "*.md"))
            {
                string id = Path.GetFileNameWithoutExtension(f);
                int dash = id.IndexOf('-');
                int n;
                if (dash > 0 && int.TryParse(id.Substring(0, dash), out n) && n > max) max = n;
            }
            return (max + 1).ToString("D2") + "-";
        }

        /// <summary>保存提示词文件（自动补 .md，UTF-8 无 BOM），返回完整路径。</summary>
        public static string SavePromptFile(string id, string body)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new Exception(AppText.T("FileNameRequired"));
            if (id.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) throw new Exception(AppText.T("InvalidFileNameChars", id));
            if (id.IndexOf("..", StringComparison.Ordinal) >= 0) throw new Exception(AppText.T("InvalidFileName"));
            string file = Path.Combine(PromptDir, id.EndsWith(".md", StringComparison.OrdinalIgnoreCase) ? id : id + ".md");
            File.WriteAllText(file, body, new UTF8Encoding(false));
            return file;
        }

        /// <summary>删除提示词文件。</summary>
        public static void DeletePromptFile(string id)
        {
            string file = Path.Combine(PromptDir, id.EndsWith(".md", StringComparison.OrdinalIgnoreCase) ? id : id + ".md");
            if (File.Exists(file)) File.Delete(file);
        }

        private static string FindOnPath(string exe)
        {
            string pathEnv = Environment.GetEnvironmentVariable("PATH") ?? "";
            foreach (string dir in pathEnv.Split(';'))
            {
                if (string.IsNullOrWhiteSpace(dir)) continue;
                try
                {
                    string cand = Path.Combine(dir.Trim('"'), exe);
                    if (File.Exists(cand)) return cand;
                }
                catch { }
            }
            return null;
        }

        private static string RunQuick(string file, string args)
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo(file, args)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                using (var p = System.Diagnostics.Process.Start(psi))
                {
                    if (!p.WaitForExit(2000)) { try { p.Kill(); } catch { } return null; }
                    string outp = p.StandardOutput.ReadToEnd().Trim();
                    return p.ExitCode == 0 ? outp : null;
                }
            }
            catch { return null; }
        }

        public static string ProjClaudeMd()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "CLAUDE.md");
        }

        public static string Apply(string promptFile, string target)
        {
            return ApplyMultiple(new[] { promptFile }, target);
        }

        public static string ApplyMultiple(IEnumerable<string> promptFiles, string target)
        {
            string dir = Path.GetDirectoryName(target);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string bak = target + ".bak";
            bool needBackup = File.Exists(target) && !IsGenerated(target);
            if (needBackup) File.Copy(target, bak, true);

            var sb = new StringBuilder();
            bool first = true;
            foreach (string f in promptFiles)
            {
                if (!first) sb.Append("\r\n\r\n---\r\n\r\n");
                sb.Append(File.ReadAllText(f, Encoding.UTF8));
                first = false;
            }

            string template = "# 项目自动加载规则（由 Claude Code 提示词管家生成）\r\n\r\n> 生成时间：{DATE}\r\n> 执行 restore 可恢复原状。\r\n\r\n## 核心规则\r\n\r\n{INJECTED_RULES}\r\n";
            if (!string.IsNullOrEmpty(TemplateFile) && File.Exists(TemplateFile))
                template = File.ReadAllText(TemplateFile, Encoding.UTF8);
            string content = template
                .Replace("{DATE}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{INJECTED_RULES}", sb.ToString());
            File.WriteAllText(target, content, new UTF8Encoding(false));
            return needBackup ? bak : null;
        }

        public static bool IsGenerated(string target)
        {
            if (!File.Exists(target)) return false;
            string content = File.ReadAllText(target, Encoding.UTF8);
            return content.Contains(GenMarker);
        }

        public static void Backup(string target)
        {
            if (!File.Exists(target)) throw new FileNotFoundException(AppText.T("TargetMissing", target));
            File.Copy(target, target + ".bak", true);
        }

        public static string Restore(string target)
        {
            string bak = target + ".bak";
            if (File.Exists(bak))
            {
                File.Copy(bak, target, true);
                return AppText.T("RestoredBackup", bak, target);
            }
            if (!File.Exists(target))
                throw new FileNotFoundException(AppText.T("NoBackupAndTargetMissing", bak));
            if (IsGenerated(target))
            {
                File.Delete(target);
                return AppText.T("DeletedGenerated", target);
            }
            throw new FileNotFoundException(AppText.T("NoBackupNonGenerated", bak));
        }
    }
}
