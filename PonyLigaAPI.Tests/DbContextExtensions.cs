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
                passwordHash = "123123123",
                userPrivileges = 0
            });

            dbContext.Groups.Add(new Group
            {
                name = "PonyGroup",
                rule = 1,
                groupSize = 3,
                participants = "Merle, Jonas"
            });

            dbContext.Teams.Add(new Team
            {
                club = "München",
                name = "PonyGroup",
                place = "München",
                consultor = "Jürgen",
                teamSize = 2,
                groupId = 1
            });

            dbContext.SaveChanges();
        }
    }
}
