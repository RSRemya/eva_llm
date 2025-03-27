using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

namespace Eva_Web.Utils
{

    public class SendEmail
    {
        String status = "";

        public void send(string to, string subject, string body)
        {
            try
            {
                string status = "";
                try
                {
                    //todo this has to change later to hello@meetevahere.com
                    var fromAddress = new MailAddress("hellomeetevahere@gmail.com", "Eva");
                    const string fromPassword = "tsmc bgjt lggh iofj";

                    var toAddress = new MailAddress(to);


                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };

                    using (var emailMessage = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml=true
                    })
                    {
                        smtp.Send(emailMessage);
                    }
                    status = "success";

                }
                catch (Exception ex)
                {
                    status = "failed";
                    Console.WriteLine($"Exception - {ex}");
                }

            }
            catch (Exception ex)
            {
                status = "failed";
                Console.WriteLine($"Exception - {ex}");
            }
        }

    }


}
