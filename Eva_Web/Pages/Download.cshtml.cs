using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eva_Web.Pages
{
    [Authorize(Roles = "Admin")]
    public class DownloadModel : PageModel
    {
        private string[] acceptableKeys = { "A7m!p_P3zQr8@", "tX24#dJ9zWq", "mN8@gkC5r%Uv" };//, "Z1bLxdd6!Oj7", "qW2ff!aT7@Vn3", "R9k#fhnL3$Yv2", "yM5!dfpG8@Xh4", "J6r#z4Q2!Nl9", "K4v@ghtD1$Sw7", "eH3#oL6g!Yk5", "F7b!v_N9@Pq2", "zT1$kW128!Gm6", "pR4!__nJ2@Lx7", "X9m#f45B3!Vq5", "oL2!tHf6@Wk9", "yD5y#vM8!Rp3", "K7c@zQ1d!Ln4", "N2j#2tP6!Wr8", "gH3@bfL9!Yx5", "T4b!nKs1@Wq7", "pL2!hmT8@Vh6", "R7f#jD5r!Xn3", "zQ6@kLg9!Op4", "yHss1!tW3@Nv8", "M5r#jupX2!Lq7" };
        public IActionResult OnGet(string passKey)
        {

            if (passKey != acceptableKeys[Random.Shared.Next(0, acceptableKeys.Length)])
                return Forbid();

            string dbName = "eva.db";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "DB", dbName);
            var backUpPath = Path.Combine(Directory.GetCurrentDirectory(), "DB", dbName + "_backup");

            if (!System.IO.File.Exists(filePath))
                return NotFound("The requested file was not found.");

            try
            {   //We take this approach because the original db file will be locked by other processes 
                System.IO.File.Copy(filePath, backUpPath, overwrite: true);
                var fileBytes = System.IO.File.ReadAllBytes(backUpPath);
                System.IO.File.Delete(backUpPath);
                return File(fileBytes, "application/octet-stream", dbName);
            }
            catch (Exception ex) { return this.StatusCode(500); }
        }


    }
}
