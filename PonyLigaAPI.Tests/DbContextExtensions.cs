using PonyLigaAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyLigaAPI.Tests
{
    public static class DbContextExtensions
    {
        public static void Seed(this PonyLigaAPIContext dbContext)
        {
            dbContext.Users.Add(new User
            {
                firstName = "Test",
                surName = "Test",
                loginName = "Test",
                passwordHash = "123123"
            });

            dbContext.SaveChanges();
        }
    }
}
