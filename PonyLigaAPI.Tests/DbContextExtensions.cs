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
                loginName = "Test123",
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
                consultor = "Jürgen",
                teamSize = 2,
                groupId = 1
            });

            dbContext.Ponies.Add(new Pony
            {
                name = "Pony",
                race = "Horse",
                age = "12"
            });

            dbContext.TeamPonies.Add(new TeamPony
            {
                teamId = 1,
                ponyId = 1
            });

            dbContext.Results.Add(new Result
            {
                gameDate = new DateTime(),
                game = "Springen",
                time = "10:10:10.111",
                score = 10,
                penaltyTime = "0",
                teamId = 1,
                position = 1
            });

            dbContext.TeamMembers.Add(new TeamMember
            {
                firstName = "Hans",
                surName = "Jürgen",
                teamId = 1
            });

            dbContext.SaveChanges();
        }
    }
}
