using System;
using System.Collections.Generic;
using System.IO;

namespace SWA.Core.Configs
{
    public class Config
    {

        private string Filename { get; set; }

        public Config(string Filename)
        {
            this.Filename = Filename;
        }

        public List<string> GetSection(string section)
        {
            List<string> ls = new List<string>();
            try
            {
                using (StreamReader sr = File.OpenText(Filename))
                {
                    string s = "";
                    bool is_section = false;
                    while ((s = sr.ReadLine()) != null)
                    {
                        s = s.Trim();
                        if (!s.StartsWith("#"))
                        {

                            if (s.Equals("[/" + section + "]")) is_section = false;

                            if (is_section) ls.Add(s);

                            if (s.Equals("[" + section + "]")) is_section = true;
                        }
                    }

                    sr.Close();

                }

            }
            catch (Exception)
            {
                return ls;
            }

            return ls;
        }

        public Object Get(string section, string key)
        {
            List<string> _section = this.GetSection(section);

            if (_section.Count == 0) return null;

            foreach (string section_line in _section)
            {
                if (section_line.Contains(":"))
                {
                    string[] args = section_line.Split(':');
                    if (args.Length == 2)
                    {

                        string parameter = args[0];
                        if (parameter.Equals(key))
                        {
                            return (Object)args[1];
                        }
                    }
                }
            }
            
            return null;
        }
    }
}
