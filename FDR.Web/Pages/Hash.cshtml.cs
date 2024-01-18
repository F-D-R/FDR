using FDR.Tools.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FDR.Web.Pages
{
    public class HashModel : PageModel
    {
        private readonly Processes Processes;

        public HashModel(Processes processes)
        {
            Processes = processes;
        }

        [Required(ErrorMessage = "Folder is empty!")]
        [PageRemote(AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST", PageHandler = "ValidateFolder", ErrorMessage = "Folder doesn't exist!")]
        [BindProperty]
        public string? Folder { get; set; }

        [DisplayName("Force (aka Re-hash)")]
        [BindProperty]
        public bool Force { get; set; } = false;

        [DisplayName("Verbose output")]
        [BindProperty]
        public bool Verbose { get; set; } = false;

        public void OnGet()
        {
            Console.WriteLine("HashModel.OnGet...");
        }

        public JsonResult OnPostValidateFolder()
        {
            var folder = new DirectoryInfo(Folder!);
            return new JsonResult(folder.Exists);
        }

        public void OnPostSelectFolder()
        {
            Console.WriteLine("HashModel.OnPostSelectFolder...");
        }

        public IActionResult OnPost()
        {
            Console.WriteLine("HashModel.OnPost...");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("HashModel.OnPost ModelState is NOT valid!");
                return Page();
            }

            var folder = new DirectoryInfo(Folder!);
            if (!folder.Exists)
            {
                ModelState.AddModelError("Folder", "Folder doesn't exist!");
                return Page();
            }

            Console.WriteLine("HashModel.OnPost ModelState is valid...");

            Console.WriteLine("");
            Console.WriteLine($"Folder: {Folder}");
            Console.WriteLine($"Force: {Force}");
            Console.WriteLine($"Verbose output: {Verbose}");

            _ = Processes.Start(operation: Operation.Hash, folder: Folder, verbose: Verbose, force: Force);

            return RedirectToPage("./Output");
        }
    }
}
