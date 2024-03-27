
using System.Text.RegularExpressions;

namespace SWA.Core.Rules
{
    public abstract class Actions
    {

        public static string ExtractVariableByRegex(string message, string pattern)
        {
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

    }
}
