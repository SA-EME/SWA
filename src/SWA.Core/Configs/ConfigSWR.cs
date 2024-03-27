using System;
using System.IO;
using SWA.Core.Logs;
using SWA.Core.Rules;
using YamlDotNet.Serialization;

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

                        try
                        {
                            var deserializer = new DeserializerBuilder().Build();
                            var yaml = File.ReadAllText(file);
                            Rule rule = (Rule)deserializer.Deserialize<Rule>(yaml);
                            rule.Name = name;
                            new LogListener(rule);
                            SWALog.Write("INFO", $"Importation du fichier {file}");
                        }
                        catch (Exception ex)
                        {
                            SWALog.Write("ERR", $"Erreur lors de l'importation du fichier {file} : {ex.Message}");
                        }

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
