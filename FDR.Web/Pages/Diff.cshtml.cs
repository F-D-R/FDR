using FDR.Tools.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FDR.Web.Pages
{
    public class DiffModel : PageModel
    {
        private readonly Processes Processes;

        public DiffModel(Processes processes)
        {
            Processes = processes;
        }

        [Required(ErrorMessage = "Folder is empty!")]
        [PageRemote(AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST", PageHandler = "ValidateFolder", ErrorMessage = "Folder doesn't exist!")]
        [BindProperty]
        public string? Folder { get; set; }

        [Display(Name = "Reference folder")]
        [Required(ErrorMessage = "Reference folder is empty!")]
        [PageRemote(AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST", PageHandler = "ValidateReferenceFolder", ErrorMessage = "Reference folder doesn't exist!")]
        [BindProperty]
        public string? ReferenceFolder { get; set; }

        [Display(Name = "Verbose output")]
        [BindProperty]
        public bool Verbose { get; set; } = false;

        public void OnGet()
        {
            Console.WriteLine("DiffModel.OnGet...");
        }

        public JsonResult OnPostValidateFolder()
        {
            var folder = new DirectoryInfo(Folder!);
            return new JsonResult(folder.Exists);
        }

        public JsonResult OnPostValidateReferenceFolder()
        {
            var folder = new DirectoryInfo(ReferenceFolder!);
            return new JsonResult(folder.Exists);
        }

        public void OnPostSelectFolder()
        {
            Console.WriteLine("DiffModel.OnPostSelectFolder...");
        }

        public void OnPostSelectReferenceFolder()
        {
            Console.WriteLine("DiffModel.OnPostSelectReferenceFolder...");
        }

        public IActionResult OnPost()
        {
            Console.WriteLine("DiffModel.OnPost...");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("DiffModel.OnPost ModelState is NOT valid!");
                return Page();
            }

            var folder = new DirectoryInfo(Folder!);
            if (!folder.Exists)
            {
                ModelState.AddModelError("Folder", "Folder doesn't exist!");
                return Page();
            }

            folder = new DirectoryInfo(ReferenceFolder!);
            if (!folder.Exists)
            {
                ModelState.AddModelError("ReferenceFolder", "Reference folder doesn't exist!");
                return Page();
            }

            Console.WriteLine("DiffModel.OnPost ModelState is valid...");

            Console.WriteLine("");
            Console.WriteLine($"Folder: {Folder}");
            Console.WriteLine($"Reference folder: {ReferenceFolder}");
            Console.WriteLine($"Verbose output: {Verbose}");

            _ = Processes.Start(operation: Operation.Diff, folder: Folder, reffolder: ReferenceFolder, verbose: Verbose);

            return RedirectToPage("./Output");
        }
    }
}
