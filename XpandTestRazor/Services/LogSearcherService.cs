using System.Text.RegularExpressions;

namespace XpandTestRazor.Services
{
    public class LogSearcherService
    {
        public async Task<List<string>> FileSearchAsync(string path, string regexPattern)
        {
            var results = new List<string>();
            var regex = new Regex(regexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string[] logFiles = Directory.GetFiles(path, "*.log");
            foreach (var file in logFiles)
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (regex.IsMatch(line))
                        {
                            results.Add(line);
                        }
                    }
                }
            }
            return results;
        }

        public string GetRexeg(string query)
        {
            query = query.Replace("*", ".*").Replace("?", ".");
            query = Regex.Replace(query, @"\s+and\s+", ")(");
            query = Regex.Replace(query, @"\s+or\s+", "|");

            return "(" + query + ")";
        }
    }
}
