namespace Eva_Web.Models
{

    public class InvitationTableModel
    {
        public Guid requestId { get; set; }
        public string emailText { get; set; }
        public string ques1Text { get; set; }
        public string ques2Text { get; set; }
    }

    public class FeatureTableModel
    {
        public Guid featureId { get; set; }
        public string firstNameText { get; set; }
        public string lastNameText { get; set; }
        public string emailText { get; set; }
        public string messageText { get; set; }
    }

}