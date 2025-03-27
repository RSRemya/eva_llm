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


namespace Eva_Web.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminModel : EvaPage
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public List<InvitationTableModel> InviteModel { get; set; }

        public List<FeatureTableModel> FeatureModel { get; set; }

        public int ActiveUserCount { get; set; }

        public int TotalUserCount { get; set; }

        

        public AdminModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager):base(signInManager) 
        {
            _userManager = userManager;
           
        }

        public async Task OnGetAsync()
        { 
            await loadData();
        }

        public override async Task<IActionResult> OnPostAsync()
        {
            var action = Request.Form["action"];  // captures the action of the user for the selected row
            if(action=="logout")
                return await base.OnPostAsync();

            var id = Request.Form["itemId"];  // captures the ID of the selected row

            var context = new EvaDbContext();
            UserRequest request = null;
            Contacts contact = null;

            if (action == "accept" || action == "reject")
                foreach (var r in context.UserRequests)
                { //we are looping because for some reason Linq's Where clause is not working for UserRequests and Contacts here
                    if (r.RequestId.ToString() == id)
                    {
                        request = r;
                        break;
                    }
                }


            /// Invitation section's actions 
            if (action == "accept")
            {
                var sendEmail = new SendEmail();
                var generatorurl = Url.Page("/UserGenerator/Usergenerator", null, null, Request.Scheme, Request.Host.ToString()); 
                var mailtemplate=Path.Combine(Directory.GetCurrentDirectory(), "Resource", "evamail.html");
                var emailFilecontent= System.IO.File.ReadAllText(mailtemplate).Replace("{0}", generatorurl); 
                sendEmail.send(request.UserEmail, "Your Journey with Eva Starts Now  🌟", emailFilecontent);
                //todo the mail body is an html page, should be under assets for use
                // sendEmail.send(request.UserEmail, "Your Journey with Eva Starts Now!!", $"<html><head><meta content=\"text/html; charset=UTF-8\" http-equiv=\"content-type\"><style type=\"text/css\">.lst-kix_p6zy4yoz81zj-2>li:before{{content:\"\\0025a0   \"}}.lst-kix_p6zy4yoz81zj-3>li:before{{content:\"\\0025cf   \"}}ul.lst-kix_kjzd01h22baz-4{{list-style-type:none}}ul.lst-kix_kjzd01h22baz-5{{list-style-type:none}}ul.lst-kix_kjzd01h22baz-6{{list-style-type:none}}ul.lst-kix_kjzd01h22baz-7{{list-style-type:none}}ul.lst-kix_kjzd01h22baz-8{{list-style-type:none}}.lst-kix_p6zy4yoz81zj-4>li:before{{content:\"\\0025cb   \"}}.lst-kix_26nt5l89k7gi-0>li:before{{content:\"\\0025cf   \"}}.lst-kix_p6zy4yoz81zj-1>li:before{{content:\"\\0025cb   \"}}.lst-kix_26nt5l89k7gi-1>li:before{{content:\"\\0025cb   \"}}.lst-kix_26nt5l89k7gi-2>li:before{{content:\"\\0025a0   \"}}.lst-kix_p6zy4yoz81zj-0>li:before{{content:\"\\0025cf   \"}}.lst-kix_kjzd01h22baz-8>li:before{{content:\"\\0025a0   \"}}.lst-kix_p6zy4yoz81zj-5>li:before{{content:\"\\0025a0   \"}}.lst-kix_p6zy4yoz81zj-7>li:before{{content:\"\\0025cb   \"}}ul.lst-kix_kjzd01h22baz-0{{list-style-type:none}}ul.lst-kix_kjzd01h22baz-1{{list-style-type:none}}.lst-kix_kjzd01h22baz-7>li:before{{content:\"\\0025cb   \"}}ul.lst-kix_kjzd01h22baz-2{{list-style-type:none}}.lst-kix_p6zy4yoz81zj-6>li:before{{content:\"\\0025cf   \"}}ul.lst-kix_kjzd01h22baz-3{{list-style-type:none}}ul.lst-kix_p6zy4yoz81zj-1{{list-style-type:none}}ul.lst-kix_p6zy4yoz81zj-2{{list-style-type:none}}ul.lst-kix_p6zy4yoz81zj-0{{list-style-type:none}}.lst-kix_p6zy4yoz81zj-8>li:before{{content:\"\\0025a0   \"}}ul.lst-kix_26nt5l89k7gi-2{{list-style-type:none}}ul.lst-kix_26nt5l89k7gi-1{{list-style-type:none}}.lst-kix_kjzd01h22baz-0>li:before{{content:\"\\0025cf   \"}}.lst-kix_kjzd01h22baz-1>li:before{{content:\"\\0025cb   \"}}ul.lst-kix_p6zy4yoz81zj-7{{list-style-type:none}}ul.lst-kix_26nt5l89k7gi-4{{list-style-type:none}}ul.lst-kix_p6zy4yoz81zj-8{{list-style-type:none}}ul.lst-kix_26nt5l89k7gi-3{{list-style-type:none}}ul.lst-kix_p6zy4yoz81zj-5{{list-style-type:none}}ul.lst-kix_26nt5l89k7gi-6{{list-style-type:none}}ul.lst-kix_p6zy4yoz81zj-6{{list-style-type:none}}ul.lst-kix_26nt5l89k7gi-5{{list-style-type:none}}.lst-kix_kjzd01h22baz-3>li:before{{content:\"\\0025cf   \"}}ul.lst-kix_p6zy4yoz81zj-3{{list-style-type:none}}ul.lst-kix_26nt5l89k7gi-8{{list-style-type:none}}ul.lst-kix_p6zy4yoz81zj-4{{list-style-type:none}}ul.lst-kix_26nt5l89k7gi-7{{list-style-type:none}}.lst-kix_kjzd01h22baz-4>li:before{{content:\"\\0025cb   \"}}.lst-kix_kjzd01h22baz-5>li:before{{content:\"\\0025a0   \"}}.lst-kix_kjzd01h22baz-6>li:before{{content:\"\\0025cf   \"}}.lst-kix_kjzd01h22baz-2>li:before{{content:\"\\0025a0   \"}}ul.lst-kix_26nt5l89k7gi-0{{list-style-type:none}}.lst-kix_26nt5l89k7gi-3>li:before{{content:\"\\0025cf   \"}}.lst-kix_26nt5l89k7gi-4>li:before{{content:\"\\0025cb   \"}}.lst-kix_26nt5l89k7gi-7>li:before{{content:\"\\0025cb   \"}}.lst-kix_26nt5l89k7gi-5>li:before{{content:\"\\0025a0   \"}}.lst-kix_26nt5l89k7gi-6>li:before{{content:\"\\0025cf   \"}}li.li-bullet-0:before{{margin-left:-18pt;white-space:nowrap;display:inline-block;min-width:18pt}}.lst-kix_26nt5l89k7gi-8>li:before{{content:\"\\0025a0   \"}}ol{{margin:0;padding:0}}table td,table th{{padding:0}}.c11{{padding-top:0pt;padding-bottom:0pt;line-height:1.15;orphans:2;widows:2;text-align:left;height:11pt}}.c4{{color:#000000;font-weight:400;text-decoration:none;vertical-align:baseline;font-size:11pt;font-family:\"Arial\";font-style:normal}}.c5{{color:#000000;text-decoration:none;vertical-align:baseline;font-size:11pt;font-family:\"Arial\";font-style:normal}}.c2{{padding-top:12pt;padding-bottom:12pt;line-height:1.15;orphans:2;widows:2;text-align:left}}.c10{{text-decoration-skip-ink:none;-webkit-text-decoration-skip:none;color:#1155cc;text-decoration:underline}}.c6{{background-color:#ffffff;max-width:468pt;padding:72pt 72pt 72pt 72pt}}.c7{{padding:0;margin:0}}.c3{{margin-left:72pt;padding-left:0pt}}.c1{{color:inherit;text-decoration:inherit}}.c9{{margin-left:36pt;padding-left:0pt}}.c0{{font-weight:700}}.c12{{height:11pt}}.c8{{font-style:italic}}.title{{padding-top:0pt;color:#000000;font-size:26pt;padding-bottom:3pt;font-family:\"Arial\";line-height:1.15;page-break-after:avoid;orphans:2;widows:2;text-align:left}}.subtitle{{padding-top:0pt;color:#666666;font-size:15pt;padding-bottom:16pt;font-family:\"Arial\";line-height:1.15;page-break-after:avoid;orphans:2;widows:2;text-align:left}}li{{color:#000000;font-size:11pt;font-family:\"Arial\"}}p{{margin:0;color:#000000;font-size:11pt;font-family:\"Arial\"}}h1{{padding-top:20pt;color:#000000;font-size:20pt;padding-bottom:6pt;font-family:\"Arial\";line-height:1.15;page-break-after:avoid;orphans:2;widows:2;text-align:left}}h2{{padding-top:18pt;color:#000000;font-size:16pt;padding-bottom:6pt;font-family:\"Arial\";line-height:1.15;page-break-after:avoid;orphans:2;widows:2;text-align:left}}h3{{padding-top:16pt;color:#434343;font-size:14pt;padding-bottom:4pt;font-family:\"Arial\";line-height:1.15;page-break-after:avoid;orphans:2;widows:2;text-align:left}}h4{{padding-top:14pt;color:#666666;font-size:12pt;padding-bottom:4pt;font-family:\"Arial\";line-height:1.15;page-break-after:avoid;orphans:2;widows:2;text-align:left}}h5{{padding-top:12pt;color:#666666;font-size:11pt;padding-bottom:4pt;font-family:\"Arial\";line-height:1.15;page-break-after:avoid;orphans:2;widows:2;text-align:left}}h6{{padding-top:12pt;color:#666666;font-size:11pt;padding-bottom:4pt;font-family:\"Arial\";line-height:1.15;page-break-after:avoid;font-style:italic;orphans:2;widows:2;text-align:left}}</style></head><body class=\"c6 doc-content\"><p class=\"c2\"><span class=\"c0\"></p><p class=\"c2\"><span class=\"c4\">Hello!</span></p><p class=\"c2\"><span>Thank you for signing up to be part of </span><span class=\"c8\">Meet Eva Here</span><span class=\"c4\">! This project is a social experiment and art piece exploring human-AI interactions. Your conversations with Eva will contribute to a deeper understanding of how people connect with AI and may be anonymously used for artistic and research purposes.</span></p><p class=\"c2\"><span>Exciting plans are ahead! Eva will be featured at </span><span class=\"c10 c0\"><a class=\"c1\" href=\"https://www.google.com/url?q=https://artsg.com/platform/meet-eva-here/&amp;sa=D&amp;source=editors&amp;ust=1733839806766094&amp;usg=AOvVaw0BLaoYUPUUbesSDxDdXNBy\">Art SG 2025</a></span><span>&nbsp;under the Platform sector with Columns Gallery, as well as other exhibitions around the world next year. Want to follow along and get exclusive updates? </span><span class=\"c10\"><a class=\"c1\" href=\"https://www.google.com/url?q=https://www.shavonnewong.art/newsletter&amp;sa=D&amp;source=editors&amp;ust=1733839806766258&amp;usg=AOvVaw2caShB4NmBJ_VJfNTs97S9\">[</a></span><span class=\"c10 c0\"><a class=\"c1\" href=\"https://www.google.com/url?q=https://www.shavonnewong.art/newsletter&amp;sa=D&amp;source=editors&amp;ust=1733839806766342&amp;usg=AOvVaw3ZOs0mFQoGg3UG5rkDffIj\">Sign up for Shavonne Wong&rsquo;s newsletter</a></span><span class=\"c10\"><a class=\"c1\" href=\"https://www.google.com/url?q=https://www.shavonnewong.art/newsletter&amp;sa=D&amp;source=editors&amp;ust=1733839806766409&amp;usg=AOvVaw3mc4BDwUx4Jgqkg0qK2kra\">]</a></span><span class=\"c4\">&nbsp;to stay in the loop.</span></p><p class=\"c2\"><span class=\"c4\">Here&rsquo;s what you need to know before getting started:</span></p><ul><li class=\"c2 c9 li-bullet-0\"><span class=\"c0\">Anonymity</span><span class=\"c4\">: Your identity remains private. No tracking is done to identify participants, as this project focuses on collective insights, not individuals.</span></li><li class=\"c2 c9 li-bullet-0\"><span class=\"c0\">Purpose</span><span class=\"c4\">: Eva is designed to spark conversations about AI and humanity. Feel free to share your thoughts and explore meaningful topics, but avoid using Eva for anything illegal, harmful, or offensive. Let&rsquo;s keep things impactful and out of trouble!</span></li><li class=\"c2 c9 li-bullet-0\"><span class=\"c0\">Username &amp; Password</span><span>: You&rsquo;ll choose a username/password combination in the next step. </span><span class=\"c0\">Save it somewhere safe</span><span class=\"c4\">&nbsp;to maintain your conversation history. Multiple accounts aren&rsquo;t recommended, as it disrupts the flow of the project.</span></li><li class=\"c2 c9 li-bullet-0\"><span class=\"c0\">Consent and Agreement</span><span class=\"c4\">: By proceeding, you acknowledge the following:</span></li></ul><ul style=\"list-style-type: circle;\"><li class=\"c2 c3 li-bullet-0\"><span class=\"c4\">You confirm that you are 18 years of age or older. This platform is not intended for use by individuals under the age of 18</span></li><li class=\"c2 c3 li-bullet-0\"><span class=\"c4\">Conversations may be anonymously stored and used for artistic and research purposes.</span></li><li class=\"c2 c3 li-bullet-0\"><span class=\"c4\">No personally identifiable information is collected or tracked, ensuring your privacy.</span></li><li class=\"c2 c3 li-bullet-0\"><span class=\"c4\">You agree to use Eva respectfully and in accordance with all applicable laws. The project team is not liable for any misuse of the platform.</span></li><li class=\"c2 c3 li-bullet-0\"><span class=\"c4\">All content generated as part of this project remains the intellectual property of Shavonne Wong and collaborators.</span></li></ul><p class=\"c2\"><span>Click below to acknowledge your consent, choose your username and password, and start chatting with Eva:<br></span><span class=\"c0\"><a href=\"{generatorurl}\">I Consent and Choose My Login</a></span><span class=\"c4\"></span></p><p class=\"c2\"><span>Thanks for being part of this exciting journey, we can&rsquo;t wait to see what unfolds!<br>The </span><span class=\"c8\">Meet Eva Here</span><span class=\"c4\">&nbsp;Team</span></p><p class=\"c2 c12\"><span class=\"c0 c5\"></span></p><p class=\"c11\"><span class=\"c4\"></span></p></body></html>");
                request.IsApproved = true;
                TempData["SuccessMessage"] = $"Access for '{request.UserEmail}' has been approved";
            }
            else if (action == "reject")
            {
                request.IsApproved = false;
                TempData["RejectMessage"] = $"Access for '{request.UserEmail}' has been rejected";
            }
            else if (action == "remove")
            {
                foreach (var c in context.Contact)
                {
                    if (c.ContactId.ToString() == id)
                    {
                        contact = c;
                        break;
                    }
                }
                contact.ShowHide = true;
                TempData["RemoveMessage"] = $"Feedback of '{contact.FirstName} {contact.LastName}' has been removed";
            }
            context.SaveChanges();
            context.Dispose();


            return await loadData(); // should be called after the successfull api call
        }

        private async Task<IActionResult> loadData()
        {
            using (var context = new EvaDbContext())
            {
                InviteModel = await context.UserRequests.Where(req => req.IsApproved == null).Select(n => new InvitationTableModel
                {
                    requestId = n.RequestId,
                    emailText = n.UserEmail,
                    ques1Text = n.ReasonforEva,
                    ques2Text = n.AccesstoEva
                }).ToListAsync();

                FeatureModel = await context.Contact.Where(cont => cont.ShowHide == false).Select(n => new FeatureTableModel
                {
                    featureId = n.ContactId,
                    firstNameText = n.FirstName,
                    lastNameText = n.LastName,
                    emailText = n.Email,
                    messageText = n.Message
                }).ToListAsync();

                ActiveUserCount = context.Conversations.Where(conv=>conv.Date.Date>=DateTime.UtcNow.AddDays(-7).Date).Select(u=>u.UserId).Distinct().Count();
                TotalUserCount = _userManager.Users.Count();
            }
            return Page();
        }
       
    }
}
