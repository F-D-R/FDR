using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FDR.Web.Pages
{
    public class FolderModel : PageModel
    {
        public void OnGet()
        {
        }

        public JsonResult OnPostValidateFolder(string? folder)
        {
            Console.WriteLine($"FolderModel.OnPostValidateFolder... {folder}");

            if (!string.IsNullOrWhiteSpace(folder)) 
                return new JsonResult(new DirectoryInfo(folder).Exists);
            return new JsonResult(false);
        }

        public IActionResult OnGetDrives(string? folder)
        {
            Console.WriteLine($"FolderModel.OnGetDrives... {folder}");

            var di = new DirectoryInfo(folder??"/");
            if (!di.Exists) { di = new DirectoryInfo("/"); }

            return Partial("_Drives", di);
        }

        public IActionResult OnGetSubfolders(string? folder)
        {
            Console.WriteLine($"FolderModel.OnGetSubfolders... {folder}");

            var di = new DirectoryInfo(folder??"/");
            if (!di.Exists) { di = new DirectoryInfo("/"); }

            return Partial("_Subfolders", di);
        }

    }
}
