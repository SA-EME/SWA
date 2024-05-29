using System.IO;
using System;
using YamlDotNet.Serialization;
using System.Diagnostics;

namespace SWA.Core
{

    public class Config
    {

        private static string ConfigPath { get; } = Path.Combine(SWALog.WorkingPath, "config.yaml");

        public string Server { get; set; }
        public string Mode { get; set; }
        public bool EnableLog { get; set; }
        public int LogLineMax { get; set; }
        public string TemplatePath { get; set; }

        public Config()
        {
        }

        public static Config getConfig()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    var deserializer = new DeserializerBuilder().Build();
                    var yaml = File.ReadAllText(ConfigPath);
                    Config config = (Config)deserializer.Deserialize<Config>(yaml);
                    return config;
                }
            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "SWA";
                    eventLog.WriteEntry("Error during loading config.yaml: " + ex.Message, EventLogEntryType.Error, 0);
                }
                Process.GetCurrentProcess().Kill();
            }
            return null;

        }

    }

}
