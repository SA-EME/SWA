using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using SWA.Core.Configs;
using SWA.Core.Logs;

namespace SWA.Core
{
    public static class SWALog
    {


        public static readonly string WorkingPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static readonly Config SWAConfig = Config.getConfig();


        public static readonly string TemplatePath = Path.Combine(WorkingPath, "Templates");

        
        private static readonly FileVersionInfo FVI = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
        private static readonly string Version = FVI.FileVersion;

        public static void Start()
        {
            new ConfigSWR(TemplatePath);
            Write("INFO", "Initialisation du service SWA " + Version);
        }


        public static void Write(string type, string msg)
        {
            if (!SWAConfig.EnableLog)
            {
                return;
            }

            if (SWAConfig.Mode.ToUpper() != "DEBUG" && type.ToUpper() == "DEBUG")
            {
                return;
            }

            string fileName = "log.txt";
            string fullPath = Path.Combine(WorkingPath, fileName);

            List<string> data = new List<string>();
            data.Add($"[{type}] {msg}");

            try
            {
                lock (LogListener.lockObject)
                {
                    if (File.Exists(fullPath))
                    {
                        data.AddRange(File.ReadAllLines(fullPath));
                    }

                    File.WriteAllLines(fullPath, data);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Erreur d'écriture dans le fichier de log : {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur inattendue : {ex.Message}");
            }
        }

    }
}
