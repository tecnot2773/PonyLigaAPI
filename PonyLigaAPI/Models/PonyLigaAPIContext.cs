using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Text.RegularExpressions;


namespace PonyLigaAPI.Models
{
    public class PonyLigaAPIContext : DbContext
    {

        public PonyLigaAPIContext(DbContextOptions<PonyLigaAPIContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = BuildConnectionString();
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }

        public DbSet<User> User { get; set; }
        public DbSet<ApiKey> ApiKey { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.firstName);
                entity.Property(e => e.surName);
                entity.Property(e => e.loginName);
                entity.Property(e => e.passwordHash);
            });
        }

        public static string BuildConnectionString()
        {
            // "server=localhost;database=library;user=mysqlschema;password=mypassword"
            string connectionString = "";
            string envConnectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb").ToString();
            string regex = @".*=(\w*);.*=(.*):(.*);.*=(.*);.*=(.*)";

            Regex rg = new Regex(regex);
            MatchCollection matches = rg.Matches(envConnectionString);

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                string database = groups[1].ToString();
                string server = groups[2].ToString();
                string port = groups[3].ToString();
                string user = groups[4].ToString();
                string password = groups[5].ToString();

                connectionString = "server=" + server + ";userid=" + user + ";password=" + password + ";database=" + database + ";port=" + port;
                connectionString = "server=" + server + ";port=" + port + ";database=pony_liga;user=" + user + ";password=" + password;
            }


            return connectionString;
        }
    }
}
