namespace Eva_Utilities
{

    using UglyToad.PdfPig;
    public static class Util
    {
        public static string ExtractTextFromPdfs(string pdfDirPaths)
        {
            string textContent = "";
            DirectoryInfo d = new DirectoryInfo(pdfDirPaths);

            foreach (var pdfFilePath in d.GetFiles().Where(f=>f.Extension==".pdf").Select(f => f.FullName))
            {
                using (var document = PdfDocument.Open(pdfFilePath))
                {
                    foreach (var page in document.GetPages())
                    {
                        textContent += page.Text; // Extract text from each page
                    }
                }
            }

            return textContent;
        }

    }
}
