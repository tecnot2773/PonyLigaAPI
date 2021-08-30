using System;
using System.Threading.Tasks;
using Xunit;
using PonyLigaAPI.Controllers;
using PonyLigaAPI.Models;

namespace PonyLigaAPI.Tests
{
    [Collection("Sequential")]
    public class TeamControllerUnitTest
    {
        [Fact]
        public async Task TestGetTeamsAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamController(dbContext);

            var response = await controller.GetTeams();

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.NotEmpty(value);
        }

        [Fact]
        public async Task TestGetTeamAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamController(dbContext);

            var response = await controller.GetTeam(1);

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, value.id);
        }

        [Fact]
        public async Task TestGetTeamAsyncFalse()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamController(dbContext);

            var response = await controller.GetTeam(2);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }


        [Fact]
        public async Task TestPutTeamAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamController(dbContext);

            var id = 1;
            var newName = "New Name";
            var team = await controller.GetTeam(id);

            team.Value.name = newName;

            var response = await controller.PutTeam(id, team.Value);

            team = await controller.GetTeam(id);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(newName, team.Value.name);
        }

        [Fact]
        public async Task TestPostTeamAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamController(dbContext);

            var team = new Team
            {
                club = "München 2",
                name = "PonyGroup München 2",
                place = "München",
                consultor = "Peter",
                teamSize = 2,
                groupId = 1
            };

            var response = await controller.PostTeam(team);

            var newTeam = await controller.GetTeam(2);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(2, newTeam.Value.id);
        }

        [Fact]
        public async Task TestDeleteTeamAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamController(dbContext);

            var response = await controller.DeleteTeam(1);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, response.Value.id);
        }
    }
}
