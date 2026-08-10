using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CCPromptLauncher
{
    public static class Cli
    {
        public static int Run(string[] args, string promptDir)
        {
            PromptLib.PromptRoot = promptDir;
            PromptLib.PromptDir = promptDir;
            PromptLib.UsePromptLanguage("zh-CN");
            PromptLib.TemplateFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inject", "CLAUDE.md.template");
            try
            {
                string action = args.Length > 0 ? args[0] : "list";
                if (action == "help" || action == "-h" || action == "--help")
                {
                    Console.WriteLine("用法: ccprompt <list|show|apply|backup|restore|export|detect> [名称...] [-t 目标文件] [-o 输出文件]");
                    Console.WriteLine("示例:");
                    Console.WriteLine("  ccprompt list                    列出全部提示词");
                    Console.WriteLine("  ccprompt show 00                 预览核心提示词");
                    Console.WriteLine("  ccprompt apply 00                启用核心提示词（用户级 CLAUDE.md，自动备份）");
                    Console.WriteLine("  ccprompt apply 00 01 03          勾选式批量启用多个提示词（按顺序合并）");
                    Console.WriteLine("  ccprompt apply 01 -t .\\CLAUDE.md 启用代码模式到项目级");
                    Console.WriteLine("  ccprompt restore -t .\\CLAUDE.md   回滚项目级");
                    Console.WriteLine("  ccprompt export 02 -o out.md      导出提示词");
                    Console.WriteLine("  ccprompt detect                  自动检测 Claude 安装/配置位置");
                    return 0;
                }
                var names = new List<string>();
                string target = null;
                string outFile = null;
                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i] == "-t" && i + 1 < args.Length) { target = args[++i]; }
                    else if (args[i] == "-o" && i + 1 < args.Length) { outFile = args[++i]; }
                    else { names.Add(args[i]); }
                }
                if (target == null) target = PromptLib.UserClaudeMd();

                switch (action)
                {
                    case "list":
                        {
                            var items = PromptLib.ListPrompts();
                            Console.WriteLine("{0,-20} {1}", "ID", "标题");
                            foreach (var p in items)
                                Console.WriteLine("{0,-20} {1}", p.Id, p.Title);
                            Console.WriteLine("共 " + items.Count + " 个提示词，目录: " + PromptLib.PromptDir);
                            return 0;
                        }
                    case "show":
                        if (names.Count == 0) { Console.Error.WriteLine("请指定提示词名称，如: ccprompt show 00"); return 2; }
                        Console.WriteLine(File.ReadAllText(PromptLib.Resolve(names[0]), Encoding.UTF8));
                        return 0;
                    case "apply":
                        {
                            if (names.Count == 0) { Console.Error.WriteLine("请指定至少一个提示词名称，如: ccprompt apply 00 01"); return 2; }
                            var files = new List<string>();
                            foreach (string n in names) files.Add(PromptLib.Resolve(n));
                            string bak = PromptLib.ApplyMultiple(files, target);
                            if (bak != null) Console.WriteLine("已备份原文件: " + bak);
                            else Console.WriteLine("目标文件不存在或已是本工具生成，直接写入（未新建 .bak；restore 可还原）");
                            Console.WriteLine("已启用 " + files.Count + " 个提示词: " + string.Join("、", names) + " -> " + target);
                            return 0;
                        }
                    case "backup":
                        PromptLib.Backup(target);
                        Console.WriteLine("已备份: " + target + ".bak");
                        return 0;
                    case "restore":
                        Console.WriteLine(PromptLib.Restore(target));
                        return 0;
                    case "detect":
                        {
                            Console.WriteLine("== Claude 安装位置检测 ==");
                            var paths = PromptLib.DetectClaudePaths(true);
                            foreach (var p in paths)
                                Console.WriteLine((p.Exists ? "[找到]   " : "[未找到] ") + (p.IsConfig ? "配置目录 · " : "程序位置 · ") + p.Label + ": " + p.Path);
                            Console.WriteLine();
                            string md = PromptLib.UserClaudeMd();
                            Console.WriteLine("=> 用户级 CLAUDE.md（apply 默认写入）: " + md + (File.Exists(md) ? "（存在）" : "（不存在，启用时自动创建）"));
                            return 0;
                        }
                    case "export":
                        {
                            if (names.Count == 0) { Console.Error.WriteLine("请指定提示词名称，如: ccprompt export 02"); return 2; }
                            string file = PromptLib.Resolve(names[0]);
                            string dest = string.IsNullOrEmpty(outFile) ? Path.Combine(Directory.GetCurrentDirectory(), names[0] + ".md") : outFile;
                            File.Copy(file, dest, true);
                            Console.WriteLine("已导出: " + dest);
                            return 0;
                        }
                    default:
                        Console.Error.WriteLine("未知命令: " + action + "，输入 ccprompt help 查看用法");
                        return 2;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("错误: " + ex.Message);
                return 1;
            }
        }
    }
}
