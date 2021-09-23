using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Text.RegularExpressions;
using PonyLigaAPI.Models;
using System.Diagnostics.CodeAnalysis;

namespace PonyLigaAPI.Models
{
    public class PonyLigaAPIContext : DbContext
    {

        public PonyLigaAPIContext(DbContextOptions<PonyLigaAPIContext> options) : base(options) { }

        [ExcludeFromCodeCoverage]
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
        public DbSet<TeamPony> TeamPonies { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(g => g.id);
                entity.Property(g => g.name);
                entity.Property(g => g.rule);
                entity.Property(g => g.groupSize);
                entity.Property(g => g.participants);
                entity.HasMany(g => g.teams).WithOne(e => e.group).HasPrincipalKey("id");
            });

            modelBuilder.Entity<Pony>(entity =>
            {
                entity.HasKey(p => p.id);
                entity.Property(p => p.name);
                entity.Property(p => p.race);
                entity.Property(p => p.age);
                //entity.HasMany(p => p.teams).WithMany(t => t.ponies).UsingEntity(t => t.ToTable("TeamPonies"));
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.HasKey(r => r.id);
                entity.Property(r => r.gameDate);
                entity.Property(r => r.game);
                entity.Property(r => r.position);
                entity.Property(r => r.time);
                entity.Property(r => r.penaltyTime);
                entity.Property(r => r.score);
                entity.HasOne(r => r.team).WithMany(r => r.results).HasForeignKey(r => r.teamId);
            });

            modelBuilder.Entity<Season>(entity =>
            {
                entity.HasKey(s => s.id);
                entity.Property(s => s.teamName);
                entity.Property(s => s.club);
                entity.Property(s => s.score);
                entity.Property(s => s.year);
                entity.Property(s => s.placement);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.id);
                entity.Property(t => t.club);
                entity.Property(t => t.name);
                entity.Property(t => t.place);
                entity.Property(t => t.consultor);
                entity.Property(t => t.teamSize);
                entity.HasOne(t => t.group).WithMany(g => g.teams).HasForeignKey(t => t.groupId);
                entity.HasMany(e => e.results).WithOne(e => e.team).HasPrincipalKey("id");
                //entity.HasMany(t => t.ponies).WithMany(p => p.teams).UsingEntity(r => r.ToTable("TeamPonies"));
            });

            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.HasKey(t => t.id);
                entity.Property(t => t.firstName);
                entity.Property(t => t.surName);
                entity.HasOne(t => t.team).WithMany(t => t.teamMembers).HasForeignKey(t => t.teamId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.id);
                entity.Property(u => u.firstName);
                entity.Property(u => u.surName);
                entity.Property(u => u.loginName);
                entity.Property(u => u.passwordHash);
                entity.Property(u => u.userPrivileges);
            });

            // Pony - Team Relation
            modelBuilder.Entity<TeamPony>(entity =>
            {
                entity.HasKey(t => new { t.ponyId, t.teamId } );
                entity.HasOne(t => t.pony).WithMany(t => t.teamPonies).HasForeignKey(t => t.ponyId);
                entity.HasOne(t => t.team).WithMany(t => t.teamPonies).HasForeignKey(t => t.teamId);
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
