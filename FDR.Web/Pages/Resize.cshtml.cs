using FDR.Tools.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FDR.Web.Pages
{
    public class ResizeModel : PageModel
    {
        private readonly Processes Processes;

        public ResizeModel(Processes processes)
        {
            Processes = processes;
        }

        public AppConfig? AppConfig { get; set; }

        [BindProperty]
        public string? ConfigKey { get; set; }

        [BindProperty]
        public ResizeConfig ResizeConfig { get; set; } = new ResizeConfig();

        [Display(Name = "Verbose output")]
        [BindProperty]
        public bool Verbose { get; set; } = false;

        [Required(ErrorMessage = "Folder is empty!")]
        [PageRemote(AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST", PageHandler = "ValidateFolder", ErrorMessage = "Folder doesn't exist!")]
        [BindProperty]
        public string? Folder { get; set; }

        public void OnGet()
        {
            Console.WriteLine("ResizeModel.OnGet...");
            try
            {
                AppConfig = AppConfig.Load();
            }
            catch { }

            ConfigKey = Request.Query["config"];
            if (AppConfig != null && !string.IsNullOrWhiteSpace(ConfigKey))
            {
                ResizeConfig = AppConfig.ResizeConfigs.Where(d => string.Equals(d.Key, ConfigKey, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Value.Clone();
            }
            if (ResizeConfig == null) { ResizeConfig = new ResizeConfig(); }
        }

        public JsonResult OnPostValidateFolder()
        {
            var folder = new DirectoryInfo(Folder!);
            return new JsonResult(folder.Exists);
        }

        public void SelectFolder()
        {
            Console.WriteLine("SelectFolder...");
        }
        public void OnPostSelectFolder()
        {
            Console.WriteLine("OnPostSelectFolder...");
        }
        public void OnGetSelectFolder()
        {
            Console.WriteLine("OnGetSelectFolder...");
        }

        public IActionResult OnPost()
        {
            Console.WriteLine("ResizeModel.OnPost...");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ResizeModel.OnPost ModelState is NOT valid!!!");
                return Page();
            }

            try { ResizeConfig.Validate(); }
            catch (Exception ex)
            {
                ModelState.AddModelError("ResizeConfig", ex.Message ?? "Invalid rename configuration!");
                return Page();
            }

            var folder = new DirectoryInfo(Folder!);
            if (!folder.Exists)
            {
                ModelState.AddModelError("Folder", "Folder doesn't exist!");
                return Page();
            }

            Console.WriteLine("ResizeModel.OnPost ModelState is valid...");

            Console.WriteLine("");
            Console.WriteLine($"Selected config: {ConfigKey}");
            Console.WriteLine($"File filter: {ResizeConfig.FileFilter}");
            Console.WriteLine($"Filename pattern: {ResizeConfig.FilenamePattern}");
            Console.WriteLine($"Filename case: {ResizeConfig.FilenameCase}");
            Console.WriteLine($"Extension case: {ResizeConfig.ExtensionCase}");
            Console.WriteLine($"Additional files: {ResizeConfig.AdditionalFiles}");
            Console.WriteLine($"Recursive: {ResizeConfig.Recursive}");
            Console.WriteLine($"Stop on error: {ResizeConfig.StopOnError}");
            Console.WriteLine($"Folder: {Folder}");
            Console.WriteLine($"Verbose output: {Verbose}");

            _ = Processes.Start(operation: Operation.Resize, folder: Folder, verbose: Verbose, tmpConfig: ResizeConfig);

            return RedirectToPage("./Output");
        }
    }
}
