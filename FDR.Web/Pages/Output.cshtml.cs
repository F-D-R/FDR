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
            Processes.RemoveAll(p => p.Task?.Status == TaskStatus.RanToCompletion);
        }

        public IActionResult OnPostCancel(int index)
        {
            Console.WriteLine($"OutputModel.OnPostCancel(index={index})");
            Processes[index]?.TokenSource?.Cancel();
            Processes.RemoveAt(index);
            return Processes.Count > 0 ? new PageResult() : RedirectToPage("./Index");
        }
    }
}
