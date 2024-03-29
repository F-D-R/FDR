using FDR.Tools.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FDR.Web.Pages
{
    public class VerifyModel : PageModel
    {
        private readonly Processes Processes;

        public VerifyModel(Processes processes)
        {
            Processes = processes;
        }

        [Required(ErrorMessage = "Folder is empty!")]
        [PageRemote(AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST", PageHandler = "ValidateFolder", ErrorMessage = "Folder doesn't exist!")]
        [BindProperty]
        public string? Folder { get; set; }

        [DisplayName("Verbose output")]
        [BindProperty]
        public bool Verbose { get; set; } = false;

        public IActionResult OnGet()
        {
            Console.WriteLine("VerifyModel.OnGet...");

            //https://localhost:7241/ajaxdemo?X-Requested-With=XMLHttpRequest&_=1705786395982
            if (Request.Query["X-Requested-With"] == "XMLHttpRequest")
            {
                return Content("Some AJAX result...", "text/plain");
            }

            return Page();
        }

        public JsonResult OnPostValidateFolder()
        {
            var folder = new DirectoryInfo(Folder!);
            return new JsonResult(folder.Exists);
        }

        public IActionResult OnPost()
        {
            Console.WriteLine("VerifyModel.OnPost...");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("VerifyModel.OnPost ModelState is NOT valid!");
                return Page();
            }

            var folder = new DirectoryInfo(Folder!);
            if (!folder.Exists)
            {
                ModelState.AddModelError("Folder", "Folder doesn't exist!");
                return Page();
            }

            Console.WriteLine("VerifyModel.OnPost ModelState is valid...");

            Console.WriteLine("");
            Console.WriteLine($"Folder: {Folder}");
            Console.WriteLine($"Verbose output: {Verbose}");

            _ = Processes.Start(operation: Operation.Verify, folder: Folder, verbose: Verbose);

            return RedirectToPage("./Output");
        }
    }
}
