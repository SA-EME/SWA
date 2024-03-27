
using System;
using System.Text.RegularExpressions;

namespace SWA.Core.Rules
{
    public abstract class Actions
    {

        public static string ExtractVariableByRegex(string message, string pattern)
        {
            SWALog.Write("DEBUG", $"ExtractVariableByRegex: {message} - {pattern}");
            Match match = Regex.Match(message, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return "N/A";
            }
        }

        public static string ExtractVariableByLine(string message, string index)
        {
            try
            {
                int _index = int.Parse(index);
                string[] lines = message.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                if (_index < lines.Length)
                {
                    return lines[_index];
                }
                else
                {
                    return "N/A";
                }
            }
            catch (FormatException)
            {
                return "N/A";
            }
        }

    }
}
