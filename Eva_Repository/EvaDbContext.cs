
/*
 * Run these commands from package manager console , use Eva_Repository as the default project
 Install-Package Microsoft.EntityFrameworkCore.Tools , only needed first time
 Add-Migration InitialCreate (creates migration classes if not already present) e.g  Add-Migration ConversationContext -Context EvaDbContext(context needs to be specified in case of multiple contexts in the project
 Update-Database (executes the migration code and creates tables) e.g Update-Database -Context EvaDbContext
 */
using Microsoft.EntityFrameworkCore;
 

namespace Eva_Repository
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserAnonymousName { get; set; }

        // Navigation properties
        public ICollection<BotStackConfigModel> BotStackConfigModels { get; set; }
        public ICollection<DiaryEntry> DiaryEntries { get; set; }
    }

    public class BotStackConfigModel
    {
        public Guid BotId { get; set; }
        public string ApiKey { get; set; }

        // Nullable foreign key
        public Guid? UserId { get; set; }

        // Navigation property
        public User User { get; set; }
    }

    public class DiaryEntry
    {
        public Guid UserId { get; set; }
        public DateTime DiaryEntryDate { get; set; }
        public string Entry { get; set; }

        // Navigation property
        public User User { get; set; }
    }

    public class Contacts
    {
        public Guid ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool ShowHide { get; set; }
        public DateTime Date { get; set; }
       
    }
    public class UserRequest
    {
        public Guid RequestId { get; set; }
        public string UserEmail { get; set; }
        public string ReasonforEva { get; set; }
        public string AccesstoEva { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime Date { get; set; }


    }

    public class ConversationContext {
        //conversation id, conversation => id is needed to sort the conversation 
        public int ConversationId { get; set; }
        public Guid UserId { get; set; }   
        public string Conversation { get; set; } 
        public DateTime Date { get; set; }

    }

    public class CharacterDefinitionContext 
    {        
        public int CharacterId { get; set; }
        public string SystemPrompt { get; set; }
        public string CombinedKB { get; set; }
        public string KbFileNames { get; set; }
        public string DiaryPrompt { get; set; }

    }

    public class EvaDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<BotStackConfigModel> BotStackConfigModels { get; set; }
        public DbSet<DiaryEntry> DiaryEntries { get; set; }
        public DbSet<Contacts> Contact { get; set; }
        public DbSet<UserRequest> UserRequests { get; set; }

        public DbSet<ConversationContext> Conversations { get; set; }

        public DbSet<CharacterDefinitionContext> CharacterDefinitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User entity
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            // Configure BotStackConfigModel entity
            modelBuilder.Entity<BotStackConfigModel>()
                .HasKey(b => b.BotId);

            modelBuilder.Entity<BotStackConfigModel>()
                .HasOne(b => b. User)
                .WithMany(u => u.BotStackConfigModels)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure DiaryEntry entity
            modelBuilder.Entity<DiaryEntry>()
                .HasKey(d => new { d.UserId, d.DiaryEntryDate });

            modelBuilder.Entity<DiaryEntry>()
                .HasOne(d => d.User)
                .WithMany(u => u.DiaryEntries)
                .HasForeignKey(d => d.UserId);

            modelBuilder.Entity<Contacts>()
             .HasKey(u => u.ContactId);
            modelBuilder.Entity<UserRequest>()
             .HasKey(u => u.RequestId);

            modelBuilder.Entity<ConversationContext>()
            .HasKey(c => c.ConversationId);

            modelBuilder.Entity<ConversationContext>() 
             .Property(c => c.ConversationId)
             .ValueGeneratedOnAdd();

            modelBuilder.Entity<CharacterDefinitionContext>()
          .HasKey(c => c.CharacterId);

            modelBuilder.Entity<CharacterDefinitionContext>()
             .Property(c => c.CharacterId)
             .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
        private string DbPath { get; set; }

        private string ConnectionString { get; set; }


        private void InitializeSQlLite()
        {

            //var folder = Environment.SpecialFolder.LocalApplicationData;
            // var path = Environment.GetFolderPath(folder);            
            // DbPath = Path.Join(path, "eva.db");
            DbPath = Path.Join(Directory.GetCurrentDirectory(), "DB", "eva.db");
        }
        private void InitializeSQLServer()
        { 
            ConnectionString = @"Server=tcp:evamist.database.windows.net,1433;Initial Catalog=evadbprod;Persist Security Info=False;User ID=evaAdmin;Password=Eva@prod;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            
        }
        public EvaDbContext()
        {
            InitializeSQLServer();
        }
        public EvaDbContext(DbContextOptions<EvaDbContext> options) :base(options)
        {
            InitializeSQLServer();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
       => options.UseSqlServer(ConnectionString);
    }


}
