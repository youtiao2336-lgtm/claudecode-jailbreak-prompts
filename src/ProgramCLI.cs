using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CCPromptLauncher
{
    internal static class ProgramCLI
    {
        [DllImport("kernel32.dll")]
        private static extern uint GetConsoleOutputCP();

        private static int Main(string[] args)
        {
            try
            {
                uint cp = GetConsoleOutputCP();
                Encoding enc = (cp == 65001) ? (Encoding)new UTF8Encoding(false) : Encoding.GetEncoding((int)cp);
                Console.OutputEncoding = enc;
            }
            catch { }
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string promptDir = System.IO.Path.Combine(exeDir, "prompts");
            if (!System.IO.Directory.Exists(promptDir))
            {
                string alt = System.IO.Path.GetFullPath(System.IO.Path.Combine(exeDir, "..", "prompts"));
                if (System.IO.Directory.Exists(alt)) promptDir = alt;
            }
            return Cli.Run(args, promptDir);
        }
    }
}
