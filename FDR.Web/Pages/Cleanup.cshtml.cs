using FDR.Tools.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FDR.Web.Pages
{
    public class CleanupModel : PageModel
    {
        private readonly List<ProcessInfo> Processes;
        private CancellationTokenSource cts = new();


        public CleanupModel(List<ProcessInfo> processes)
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

        //public StreamReader Output { get; } = new StreamReader(Console.OpenStandardOutput());

        public string? Output { get; set; } = string.Empty;

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

            cts.Token.ThrowIfCancellationRequested();
            var task = new DummyProcess().Start(cts.Token);
            Processes.Add(new(Operation.Cleanup, cts, task));

            return RedirectToPage("./Output");
        }
    }

    public class OutputResult : ActionResult
    {
        private readonly TextWriter _output = Console.Out;

        public override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "text/xml";
            var responseStream = response.BodyWriter.AsStream();

            //document.Open(response.OutputStream, FormatType.Html, XHTMLValidationType.Transitional);


            return new Task(() => { });
            //return Task.CompletedTask;
        }

        //public override void ExecuteResult(ActionContext context)
        //{
        //    var response = context.HttpContext.Response;
        //    response.ContentType = "text/xml";
        //    document.Open(response.OutputStream, FormatType.Html, XHTMLValidationType.Transitional);
        //}
    }
}
