namespace Eva_Web.Models
{
    public class KBModel
        {
            public string systemPromptText { get; set; }
            public string DiaryPrompt { get;set; }
             public string SavedKBFileNames { get; set; }
            public List<IFormFile> selectedFiles { get; set; }
        }
}