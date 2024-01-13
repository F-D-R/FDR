using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FDR.Tools.Library;

namespace FDR.Web.Pages
{
    public class OutputModel : PageModel
    {
        public readonly List<ProcessInfo> Processes;

        public OutputModel(List<ProcessInfo> processes)
        {
            Processes = processes;
            Processes.Add(new ("Output", Operation.Help));
        }

        public void OnGet()
        {
        }
    }
}
