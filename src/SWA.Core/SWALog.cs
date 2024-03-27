using System;
using System.IO;
using System.Reflection;
using SWA.Core.Configs;

namespace SWA.Core
{
    public static class SWALog
    {

        public static readonly string WorkingPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public static readonly string TemplatePath = Path.Combine(WorkingPath, "Templates");

        public static readonly string Mode = "INFO";

        public static void Start()
        {
            new ConfigSWR(TemplatePath);
            Write("INFO", "Initialisation du service SWA");
        }


        public static void Write(String type, String msg)
        {
            if (Mode != "DEBUG" && type == "DEBUG")
            {
                return;
            }

            string fileName = "log.txt";
            string fullPath = Path.Combine(WorkingPath, fileName);

            string[] data = new string[3000];
            data[0] = "[" + type + "] " + msg;
            try
            {
                using (StreamReader sr = File.OpenText(fullPath))
                {
                    string s = "";
                    int i = 1;
                    while ((s = sr.ReadLine()) != null)
                    {
                        data[i] = s;
                        i++;
                    }
                    sr.Close();
                }
            }
            catch (Exception)
            {

            }


            File.WriteAllLines(fullPath, data);
        }

    }
}
