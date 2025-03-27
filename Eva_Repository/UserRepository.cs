namespace Eva_Repository
{
    public static class UserRepository
    {

        public static bool SaveOrUpdateUser(UserRequest userRequestEntry)
        {
            using (var context = new EvaDbContext())
            {
                var userRequest = new UserRequest { RequestId = userRequestEntry.RequestId, UserEmail = userRequestEntry.UserEmail, ReasonforEva = userRequestEntry.ReasonforEva, AccesstoEva = userRequestEntry.AccesstoEva, Date = DateTime.UtcNow };

                context.UserRequests.Add(userRequest);
                context.SaveChanges();
                return true;
            }
        } 


        public static bool SaveOrUpdateContact(Contacts contactEntry)
        {
            using (var context = new EvaDbContext())
            {
                var contactsData = new Contacts { ContactId = contactEntry.ContactId, FirstName = contactEntry.FirstName, LastName = contactEntry.LastName, Email = contactEntry.Email, Message = contactEntry.Message, Date = DateTime.UtcNow };

                context.Contact.Add(contactsData);
                int recordsAffected = context.SaveChanges();

                return recordsAffected > 0;
            }
        }
    }
}


