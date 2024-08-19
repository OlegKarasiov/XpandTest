using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using XpandTestRazor.Services;

namespace XpandTestRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly LogSearcherService _logSearcherService;

        public IndexModel(LogSearcherService logSearcherService)
        {
            _logSearcherService = logSearcherService;
        }

        [BindProperty]
        public string SearchPattern { get; set; }

        public List<string> SearchResults { get; private set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(SearchPattern))
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string logsDirectory = Path.Combine(baseDirectory, @"..\..\..\..\Logs");
                string fullLogsPath = Path.GetFullPath(logsDirectory);

                SearchResults = await _logSearcherService.FileSearchAsync(fullLogsPath, _logSearcherService.GetRexeg(SearchPattern));
            }
            return Page();
        }
    }
}
