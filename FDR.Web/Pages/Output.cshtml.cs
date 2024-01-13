using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FDR.Web.Pages
{
    public class OutputModel : PageModel
    {
        public readonly List<ProcessInfo> Processes;

        public OutputModel(List<ProcessInfo> processes)
        {
            Processes = processes;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCancel(int index)
        {
            Console.WriteLine($"OutputModel.OnPostCancel... (index={index})");
            Processes[index]?.TokenSource?.Cancel();
            Processes.RemoveAt(index);
            if (Processes.Count == 0) { return RedirectToPage("./Index"); }
            return new PageResult();
        }
    }
}
