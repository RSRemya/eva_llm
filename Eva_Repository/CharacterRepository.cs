using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eva_Repository
{
    public static class CharacterRepository
    {

        public static bool SaveNewCharacter(string SystemPrompt, string CombinedKB, string KbFileNames, string diaryPrompt)
        {
            using (var context = new EvaDbContext())
            {
                context.CharacterDefinitions.Add(new CharacterDefinitionContext {  SystemPrompt= SystemPrompt, CombinedKB = CombinedKB, KbFileNames=KbFileNames, DiaryPrompt=diaryPrompt });
                context.SaveChanges();
                return true;
            }
        }

        public static bool UpdateCharacterIfExists(int characterId, string SystemPrompt, string CombinedKB,string KbFileNames,string diaryPrompt)
        {
            using (var context = new EvaDbContext())
            {
                var character = context.CharacterDefinitions.Where(c => c.CharacterId == characterId);
                if (character == null || character.ToList().Count==0) 
                  return SaveNewCharacter(SystemPrompt, CombinedKB, KbFileNames, diaryPrompt);

                character.First().SystemPrompt = SystemPrompt;
           
                var KBChanged = character.First().KbFileNames!= KbFileNames; 
                if (KBChanged)
                {
                    character.First().CombinedKB = CombinedKB;
                    character.First().KbFileNames = KbFileNames;
                }
                character.First().DiaryPrompt = diaryPrompt;
              
                context.SaveChanges();
                return true;
            }
        }

        public static bool UpdateCharacter(int characterId,string SystemPrompt, string CombinedKB, string KbFileNames, string diaryPrompt)
        {
            using (var context = new EvaDbContext())
            {
                var character= context.CharacterDefinitions.First(c=>c.CharacterId==characterId);
                character.SystemPrompt= SystemPrompt;
                character.CombinedKB= CombinedKB;
                character.DiaryPrompt = diaryPrompt;
                character.KbFileNames = KbFileNames;
                context.SaveChanges();
                return true;
            }
        }
        public static CharacterDefinitionContext LoadCharacter(int characterId)
        {
            using (var context = new EvaDbContext())
            {
                return context.CharacterDefinitions.Where(c => c.CharacterId == characterId)?.First();
            }
        }
    }
}
