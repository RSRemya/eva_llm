namespace Eva_Rest_API.Model
{
    /// <summary>
    /// Model class for User, all model classes may have to be moved out to a separate project later
    /// </summary>
    public class UserModel
    {
        public Guid UserId; 
        public string UserAnonymousName;
        BotStackConfigModel botStackConfigModel;

        public bool CreateUser(string userAnonymousName)
        {
            if (DoesUserNameExist(userAnonymousName)) return false;
            //Create user
            return true;
        }
        public bool DoesUserNameExist(string userAnonymousName) { return false; }
        public UserModel LoadUser(string userName) {

            return this;
        }
        public UserModel LoadUser(Guid userId) { return this; }

        public bool DeleteUser(string userId) { return false; }
        public bool DeleteUser(Guid userId) {return false; }

        //no point in adding update methods for now

    }
}
