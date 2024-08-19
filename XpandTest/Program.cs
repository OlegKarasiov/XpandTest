using System.Text.RegularExpressions;

class Program
{

    static async Task Main(string[] args)
    {

        Console.WriteLine("Please enter your search pattern:");
        string query = Console.ReadLine();

        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string logsDirectory = Path.Combine(baseDirectory, @"..\..\..\..\Logs");
        string fullLogsPath = Path.GetFullPath(logsDirectory);
        string[] logFiles = Directory.GetFiles(fullLogsPath, "*.log");

        foreach (var file in logFiles)
        {
            try
            {
                var regexPattern = GetRexeg(query);
                var results = await FileSearchAsync(file, regexPattern);

                if (results.Count > 0)
                {
                    Console.WriteLine($"Found {results.Count} matches:");
                    foreach (var line in results)
                    {
                        Console.WriteLine(line);
                    }
                }
                else
                {
                    Console.WriteLine("No matches found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    private static string GetRexeg(string query)
    {
        query = query.Replace("*", ".*").Replace("?", ".");
        query = Regex.Replace(query, @"\s+and\s+", ")(");
        query = Regex.Replace(query, @"\s+or\s+", "|");

        return "(" + query + ")";
    }

    private static async Task<List<string>> FileSearchAsync(string filePath, string regexPattern)
    {
        var results = new List<string>();
        var regex = new Regex(regexPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        using (StreamReader reader = new StreamReader(filePath))
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

        return results;
    }
}