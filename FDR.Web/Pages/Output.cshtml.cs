using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Linq;

namespace FDR.Web.Pages
{
    public class OutputModel : PageModel
    {
        public Processes Processes { get; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Index { get; set; }

        public string? Output { get; set; }

        public ProcessInfo? SelectedProcess { get; set; }

        public OutputModel(Processes processes)
        {
            Processes = processes;
        }

        public void OnGet()
        {
            if (int.TryParse(Request.Query["id"], out int id) && Processes.Count > id)
            {
                SelectedProcess = Processes.FirstOrDefault(p => p.Id == id);
                Output = SelectedProcess?.Output.ToString();
            }
            else if (int.TryParse(Request.Query["index"], out int index) && Processes.Count > index)
            {
                SelectedProcess = Processes[index];
                Output = SelectedProcess?.Output.ToString();
            }
            else if (Processes.Count > 0)
            {
                SelectedProcess = Processes[0];
                Output = SelectedProcess.Output.ToString();
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

        public IActionResult OnGetOutput()
        {
            //var output = SelectedProcess?.Output.ToString() ?? string.Empty;
            var output = (Processes.FirstOrDefault(p => p.Id == Id))?.Output.ToString() ?? string.Empty;
            return Content(output, "text/plain");
        }
    }
}
