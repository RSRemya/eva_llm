namespace Eva_Web.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using static Eva_Web.Pages.UsergeneratorModel;

    using CrypticWizard.RandomWordGenerator; 
    public static class CredentialGenerator
    {
        public static string GenerateRandomUsername()
        {
            WordGenerator myWordGenerator = new WordGenerator(); 
            return myWordGenerator.GetWord(WordGenerator.PartOfSpeech.adj)+"_"+ myWordGenerator.GetWord(WordGenerator.PartOfSpeech.noun); 
        }

        public static string GenerateRandomPassword(int length = 12, bool includeSymbols = true)
        {
            const string lettersAndDigits = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string symbols = "!@#$%^&*()-_=+<>?";

            string characterSet = lettersAndDigits;
            if (includeSymbols)
            {
                characterSet += symbols;
            }

            var password = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < length; i++)
            {
                password.Append(characterSet[random.Next(characterSet.Length)]);
            }

            return password.ToString();
        }

        public static Dictionary<string,string> GenerateMultipleCredentials(int numberOfCredentials = 5)
        {
            //var credentialsModel = new CredentialsModel();
            var objCredential = new Dictionary<string, string>();

            for (int i = 0; i < numberOfCredentials; i++)
            {
                string username = GenerateRandomUsername();
                string password = GenerateRandomPassword(16, true);
                objCredential.Add(username, password);
            }

            return objCredential;
        }
    }

}
