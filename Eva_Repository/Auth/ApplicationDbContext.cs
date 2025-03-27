using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eva_Repository.Auth
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            InitializeSQLServer();
        }
        public ApplicationDbContext()
        {
            InitializeSQLServer();
        }
        private string ConnectionString { get; set; }
        private void InitializeSQLServer()
        {
            ConnectionString = @"Server=tcp:evamist.database.windows.net,1433;Initial Catalog=evadbprod;Persist Security Info=False;User ID=evaAdmin;Password=Eva@prod;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        }
        private string DbPath { get; set; }
     
        protected override void OnConfiguring(DbContextOptionsBuilder options)
          => options.UseSqlServer(ConnectionString);
    }

}
