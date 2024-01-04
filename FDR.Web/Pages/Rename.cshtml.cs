using FDR.Tools.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FDR.Web.Pages
{
    public class RenameModel : PageModel
    {
        private readonly IConfiguration Configuration;

        public RenameModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public AppConfig? AppConfig { get; set; }

        [BindProperty]
        public string? ConfigKey { get; set; }

        [BindProperty]
        public RenameConfig RenameConfig { get; set; } = new RenameConfig();

        [BindProperty]
        public bool Verbose { get; set; } = false;

        [Required(ErrorMessage = "Folder is empty!")]
        [PageRemote(AdditionalFields = "__RequestVerificationToken", HttpMethod = "POST", PageHandler = "CheckFolder", ErrorMessage = "FOLDER DOESN'T EXIST!")]
        [BindProperty]
        public string? Folder { get; set; }

        [BindProperty]
        public IFormFile? UploadedFile { get; set; }

        //public StreamReader Output { get; } = new StreamReader(Console.OpenStandardOutput());

        public void OnGet()
        {
            Console.WriteLine("RenameModel.OnGet...");
            try
            {
                AppConfig = AppConfig.Load();
            }
            catch { }

            ConfigKey = Request.Query["config"];
            if (AppConfig != null && !string.IsNullOrWhiteSpace(ConfigKey))
            {
                RenameConfig = AppConfig.RenameConfigs.Where(d => string.Equals(d.Key, ConfigKey, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Value;
            }
            if (RenameConfig == null) { RenameConfig = new RenameConfig(); }
        }

        public JsonResult OnPostCheckFolder()
        {
            var folder = new DirectoryInfo(Folder!);
            return new JsonResult(folder.Exists);
        }

        public void OnPost2()
        {
            Console.WriteLine("RenameModel.OnPost...");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("RenameModel.OnPost ModelState is NOT valid!!!");
                Page();
                return;
            }

            try
            {
                RenameConfig.Validate();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("RenameConfig", ex.Message);
                Page();
                return;
            }

            var folder = new DirectoryInfo(Folder!);
            if (!folder.Exists)
            {
                ModelState.AddModelError("Folder", "Folder doesn't exist!");
                Page();
                return;
            }

            Console.WriteLine("RenameModel.OnPost ModelState is valid...");

            Console.WriteLine("");
            Console.WriteLine($"Selected config: {ConfigKey}");
            Console.WriteLine($"File filter: {RenameConfig.FileFilter}");
            Console.WriteLine($"Filename pattern: {RenameConfig.FilenamePattern}");
            Console.WriteLine($"Filename case: {RenameConfig.FilenameCase}");
            Console.WriteLine($"Extension case: {RenameConfig.ExtensionCase}");
            Console.WriteLine($"Additional files: {RenameConfig.AdditionalFiles}");
            Console.WriteLine($"Recursive: {RenameConfig.Recursive}");
            Console.WriteLine($"Stop on error: {RenameConfig.StopOnError}");
            Console.WriteLine($"Verbose output: {Verbose}");
            Console.WriteLine($"Uploaded file: {UploadedFile?.FileName}");

            //Rename.RenameFilesInFolder(new DirectoryInfo(Path.GetFullPath(folder)), RenameConfig);

            //_context.Movies.Add(Movie);
            //await _context.SaveChangesAsync();

            RedirectToPage("./Import");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("RenameModel.OnPostAsync...");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("RenameModel.OnPostAsync ModelState is NOT valid!!!");
                return Page();
            }
            try { RenameConfig.Validate(); }
            catch (Exception ex)
            {
                ModelState.AddModelError("RenameConfig", ex.Message);
                return Page();
            }
            //var folder = new DirectoryInfo(Folder!);
            //if (!folder.Exists)
            //{
            //    ModelState.AddModelError("Folder", "Folder doesn't exist!");
            //    return Page();
            //}

            Console.WriteLine("RenameModel.OnPostAsync ModelState is valid...");

            Console.WriteLine("");
            Console.WriteLine($"Selected config: {ConfigKey}");
            Console.WriteLine($"File filter: {RenameConfig.FileFilter}");
            Console.WriteLine($"Filename pattern: {RenameConfig.FilenamePattern}");
            Console.WriteLine($"Filename case: {RenameConfig.FilenameCase}");
            Console.WriteLine($"Extension case: {RenameConfig.ExtensionCase}");
            Console.WriteLine($"Additional files: {RenameConfig.AdditionalFiles}");
            Console.WriteLine($"Recursive: {RenameConfig.Recursive}");
            Console.WriteLine($"Stop on error: {RenameConfig.StopOnError}");
            Console.WriteLine($"Verbose output: {Verbose}");
            Console.WriteLine($"Uploaded file: {UploadedFile?.FileName}");

            //_context.Movies.Add(Movie);
            //await _context.SaveChangesAsync();

            var sourceFilePath = "D:\\GIT\\FDR\\FDR.Web\\bin\\Release\\net7.0\\appsettings.json";
            var destFilePath = "D:\\GIT\\FDR\\FDR.Web\\bin\\Release\\net7.0\\appsettings_copy.json";

            await Common.CopyFileAsync(sourceFilePath, destFilePath);

            return RedirectToPage("./Index");
        }
    }
}
