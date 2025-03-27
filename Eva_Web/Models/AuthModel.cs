namespace Eva_Web.Models
{

    public class LoginFormModel
        {
            public string userNameText { get; set; }
            public string passwordText { get; set; }
        }

         public class RegisterFormModel
        {
            public string emailText { get; set; }
            public string passwordText { get; set; }
            public string confirmPasswordText { get; set; }
        }

}