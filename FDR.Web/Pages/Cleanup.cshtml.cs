using FDR.Tools.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FDR.Web.Pages
{
    public class CleanupModel : PageModel
    {
        private readonly Processes Processes;

        public CleanupModel(Processes processes)
        {
            Processes = processes;
        }

        [Required(ErrorMessage = "Folder is empty!")]
        [PageRemote(AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST", PageHandler = "ValidateFolder", ErrorMessage = "Folder doesn't exist!")]
        [BindProperty]
        public string? Folder { get; set; }

        [Display(Name = "Verbose output")]
        [BindProperty]
        public bool Verbose { get; set; } = false;

        public void OnGet()
        {
            Console.WriteLine("CleanupModel.OnGet...");
        }

        public JsonResult OnPostValidateFolder()
        {
            var folder = new DirectoryInfo(Folder!);
            return new JsonResult(folder.Exists);
        }

        public void OnPostSelectFolder()
        {
            Console.WriteLine("CleanupModel.OnPostSelectFolder...");
        }

        public IActionResult OnPost()
        {
            Console.WriteLine("CleanupModel.OnPost...");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("CleanupModel.OnPost ModelState is NOT valid!");
                return Page();
            }

            var folder = new DirectoryInfo(Folder!);
            if (!folder.Exists)
            {
                ModelState.AddModelError("Folder", "Folder doesn't exist!");
                return Page();
            }

            Console.WriteLine("CleanupModel.OnPost ModelState is valid...");

            Console.WriteLine("");
            Console.WriteLine($"Folder: {Folder}");
            Console.WriteLine($"Verbose output: {Verbose}");

            _ = Processes.Start(operation: Operation.Cleanup, folder: Folder, verbose: Verbose);

            return RedirectToPage("./Output");
        }
    }
}
