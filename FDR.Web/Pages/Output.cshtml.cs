using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FDR.Web.Pages
{
    public class OutputModel : PageModel
    {
        public Processes Processes { get; }

        public string? Output { get; set; }

        public OutputModel(Processes processes)
        {
            Processes = processes;
        }

        public void OnGet()
        {
            var index = 0;
            int.TryParse(Request.Query["index"], out index);
            if (Processes.Count > index)
            {
                Output = Processes[index]?.Output.ToString();
            }

            Processes.RemoveAll(p => p.Task.IsCompleted);
        }

        public IActionResult OnPostCancel(int index)
        {
            Console.WriteLine($"OutputModel.OnPostCancel(index={index})");
            Processes[index]?.CancellationTokenSource?.Cancel();
            Processes.RemoveAt(index);
            return Processes.Count > 0 ? new PageResult() : RedirectToPage("./Index");
        }
    }
}
