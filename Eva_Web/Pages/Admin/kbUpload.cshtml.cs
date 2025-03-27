using Eva_Repository;
using Eva_Repository.Auth;
using Eva_Web.Models;
using Eva_Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eva_Utilities;
using System.IO;
using Microsoft.IdentityModel.Tokens;

namespace Eva_Web.Pages
{
    [Authorize(Roles = "Admin")]
    public class KBUploadModel :  EvaPage
    {
        private readonly ILogger<KBUploadModel> _logger; 

        public KBUploadModel(ILogger<KBUploadModel> logger, SignInManager<ApplicationUser> signInManager) : base(signInManager)
        {
            _logger = logger;
        }

        [BindProperty]
        public KBModel kbModel { get; set; }
        public string PromptErrorMessage { get; set; }
        public string FilesErrorMessage { get; set; }
        public string DbStatus { get; set; }
        [BindProperty]
        public string KbFileNames { get; set; }

        public void OnGet()
        {
            var character = CharacterRepository.LoadCharacter(1);
            kbModel = new KBModel();
            kbModel.systemPromptText = character.SystemPrompt;
            kbModel.DiaryPrompt = character.DiaryPrompt;
            KbFileNames= kbModel.SavedKBFileNames = character.KbFileNames; 

        }

        public override async Task<IActionResult> OnPostAsync()
        {
            var action = Request.Form["action"];
            if (action == "logout")
                return await base.OnPostAsync();
            return await ValidateData();
        }

        private async Task<IActionResult> ValidateData()
        {
            bool failCheck = false;

            string systemPrompt = kbModel.systemPromptText;     


            if (string.IsNullOrEmpty(systemPrompt))
            {
                PromptErrorMessage = "ⓘ System Prompt is required.";
                failCheck = true;
            }

            if (string.IsNullOrEmpty(kbModel.DiaryPrompt))
            {
                PromptErrorMessage = "ⓘ Diary Prompt is required.";
                failCheck = true;
            }

            if (kbModel.selectedFiles != null && kbModel.selectedFiles.Any(f => !f.FileName.EndsWith("pdf")))
            {
                FilesErrorMessage = "ⓘ KB Files need to be in PDF format.";
                failCheck = true;
            }

            if (!failCheck)
            {
                List<string> kbPaths = new List<string>();
                string Kbtext = "";
                if (kbModel.selectedFiles != null)
                {
                    foreach (var file in kbModel.selectedFiles!)
                    {
                        var stream = new MemoryStream();
                        file.CopyTo(stream);
                        var temp = Path.Combine(Path.GetTempPath(), file.FileName);
                        kbPaths.Add(temp);
                        System.IO.File.WriteAllBytes(temp, stream.ToArray());
                    }
                    //read kb text from all temp files, 
                    Kbtext = kbModel.selectedFiles != null ? Util.ExtractTextFromPdfs(Path.GetTempPath()) : "";
                    kbModel.selectedFiles.ForEach((f) => kbModel.SavedKBFileNames += f.FileName + ",");
                    
                }
                DbStatus = CharacterRepository.UpdateCharacterIfExists(1, systemPrompt, Kbtext, kbModel.SavedKBFileNames??"", kbModel.DiaryPrompt) ? "Success" : "Failed";//1. is always Eva 
                foreach (var file in kbPaths)
                    System.IO.File.Delete(file);

            }
            return Page();


        }
    }
}
