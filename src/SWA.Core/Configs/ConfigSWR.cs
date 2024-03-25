

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

        private void Import()
        {

            try
            {
                if (Directory.Exists(Path))
                {
                    string[] files = Directory.GetFiles(Path);

                    foreach (string file in files)
                    {
                        string name = file.Substring(file.LastIndexOf("\\") + 1);
                        Config SWRConfig = new Config(file);
                        string log = (string) SWRConfig.Get("filter", "log");
                        string source = (string) SWRConfig.Get("filter", "source");
                        int event_id = Int16.Parse((string) SWRConfig.Get("filter", "event_id"));
                        SWALog.Write("DEBUG", $"Source: {source}  EventID: {event_id}");
                        /*string contains = (string) SWRConfig.Get("filter", "contains");*/

                        RuleFilter filter = new RuleFilter(log, source, event_id, null, null);

                        List<String> process = SWRConfig.GetSection("process");



                        List<String> transform = SWRConfig.GetSection("transform");

                        new LogListener(log, new Rules.Rule(name, filter, null, null));
                        SWALog.Write("INFO", $"Importation du fichier {file}");

                    }
                }
                else
                {
                    SWALog.Write("ERR", $"Le dossier {Path} n'existe pas.");
                }
            }
            catch (Exception ex)
            {
                SWALog.Write("ERR", $"Une erreur s'est produite : {ex.Message}");
            }

        }

    }
}
