using FDR.Tools.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FDR.Web.Pages
{
    public class CleanupModel : PageModel
    {
        [Required(ErrorMessage = "Folder is empty!")]
        [PageRemote(AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST", PageHandler = "ValidateFolder", ErrorMessage = "Folder doesn't exist!")]
        [BindProperty]
        public string? Folder { get; set; }

        [BindProperty]
        public bool Verbose { get; set; } = false;

        //public StreamReader Output { get; } = new StreamReader(Console.OpenStandardOutput());

        public string? Output { get; set; } = string.Empty;

        public void OnGet()
        {
            Console.WriteLine("RenameModel.OnGet...");
        }

        public JsonResult OnPostValidateFolder()
        {
            var folder = new DirectoryInfo(Folder!);
            return new JsonResult(folder.Exists);
        }

        public void OnClickSelectFolder()
        {
            //Message = "SelectFolder...";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("RenameModel.OnPostAsync...");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("RenameModel.OnPostAsync ModelState is NOT valid!");
                return Page();
            }

            var folder = new DirectoryInfo(Folder!);
            if (!folder.Exists)
            {
                ModelState.AddModelError("Folder", "Folder doesn't exist!");
                return Page();
            }

            Console.WriteLine("RenameModel.OnPostAsync ModelState is valid...");

            Console.WriteLine("");
            Console.WriteLine($"Folder: {Folder}");
            Console.WriteLine($"Verbose output: {Verbose}");

            var sourceFilePath = @"D:\GIT\FDR\FDR.Web\bin\Release\net7.0\appsettings.json";
            var destFilePath = @"D:\GIT\FDR\FDR.Web\bin\Release\net7.0\appsettings_copy.json";
            await Common.CopyFileAsync(sourceFilePath, destFilePath);

            return RedirectToPage("./Index");
        }
    }
}
