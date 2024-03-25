

using System.IO;
using System;
using System.Collections.Generic;
using SWA.Core.Logs;
using SWA.Core.Rules;

namespace SWA.Core.Configs
{
    public class ConfigSWR
    {

        private string Path { get; set; }

        public ConfigSWR(string path)
        {
            this.Path = path;
            Import();
        }

        public void Import()
        {
            /*
             * Need to import every configuration file from the path and load it into memory
             */

            try
            {
                // Vérifie si le dossier existe
                if (Directory.Exists(Path))
                {
                    // Obtient tous les fichiers
                    string[] files = Directory.GetFiles(Path);

                    // Affiche le chemin de chaque fichier
                    foreach (string file in files)
                    {
                        Config SWRConfig = new Config(file);
                        string log = (string) SWRConfig.Get("filter", "log");
                        string source = (string) SWRConfig.Get("filter", "source");
                        int event_id = (int) SWRConfig.Get("filter", "event_id");
                        string contains = (string) SWRConfig.Get("filter", "contains");

                        RuleFilter filter = new RuleFilter(log, source, event_id, contains);

                        List<String> process = SWRConfig.GetSection("process");

                        List<String> transform = SWRConfig.GetSection("transform");

                        new LogListener(log, new Rules.Rule(filter, null, null));

                    }
                }
                else
                {
                    // Shutdown the application & log the error
                    Console.WriteLine($"Le dossier {Path} n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                // Shutdown the application & log the error
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }

        }

    }
}
