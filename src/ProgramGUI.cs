using System;
using System.Windows.Forms;

namespace CCPromptLauncher
{
    internal static class ProgramGUI
    {
        [STAThread]
        private static void Main(string[] args)
        {
            AppText.Initialize(args);
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string promptDir = System.IO.Path.Combine(exeDir, "prompts");
            if (!System.IO.Directory.Exists(promptDir))
            {
                string alt = System.IO.Path.GetFullPath(System.IO.Path.Combine(exeDir, "..", "prompts"));
                if (System.IO.Directory.Exists(alt)) promptDir = alt;
            }
            PromptLib.PromptRoot = promptDir;
            PromptLib.PromptDir = promptDir;
            PromptLib.UsePromptLanguage(AppText.Code);
            PromptLib.TemplateFile = System.IO.Path.Combine(exeDir, "inject", "CLAUDE.md.template");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(promptDir));
        }
    }
}
