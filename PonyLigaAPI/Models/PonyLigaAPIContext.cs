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


        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.first_name);
                entity.Property(e => e.sur_name);
                entity.Property(e => e.login_name);
                entity.Property(e => e.password_hash);
            });
        }
    }
}
