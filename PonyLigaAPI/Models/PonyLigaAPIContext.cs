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

        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Pony> Ponies { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.name);
                entity.Property(e => e.rule);
                entity.HasMany(e => e.teams).WithOne();
                entity.Property(e => e.groupSize);
                entity.Property(e => e.participants);
            });

            modelBuilder.Entity<Pony>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.name);
                entity.Property(e => e.race);
                entity.Property(e => e.age);
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.game);
                entity.Property(e => e.position);
                entity.Property(e => e.finishingTime);
                entity.Property(e => e.startingTime);
                entity.Property(e => e.timeSum);
                entity.Property(e => e.score);
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.teamName);
                entity.Property(e => e.club);
                entity.Property(e => e.score);
                entity.Property(e => e.year);
                entity.Property(e => e.placement);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.club);
                entity.Property(e => e.name);
                entity.Property(e => e.place);
                entity.Property(e => e.consultor);
                entity.Property(e => e.teamSize);
            });

            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.firstName);
                entity.Property(e => e.surName);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.firstName);
                entity.Property(e => e.surName);
                entity.Property(e => e.loginName);
                entity.Property(e => e.passwordHash);
                entity.Property(e => e.userPrivileges);
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
