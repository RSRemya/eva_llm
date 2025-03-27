namespace Eva_Rest_API.Model
{
    public class DiaryModel
    {
        public UserModel User {get;set;}

        public List<DiaryEntry> DiaryEntries { get; set; }

        public List<DiaryEntry> GetAllDiaryEntriesForUser (UserModel user)
        {
            return DiaryEntries;
        }
        public List<DiaryEntry> GetDiaryEntriesForUserWithinDateRange(DateTime from,DateTime to)
        {
            return DiaryEntries;
        }
        //useful as input to a LLM 
        public static List<DiaryModel> GetAllDiaryEntriesForAllUsers()
        {
            return new List<DiaryModel>();
        }

        public bool MakeDiaryEntriesForUser(UserModel user,List<DiaryEntry> diaryEntries) { return true; }


    }

    public class DiaryEntry
    {
        public DateTime DiaryEntryDate { get; set; }
        public string Entry {  get; set; }
    }
}
