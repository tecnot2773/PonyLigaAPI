using System;
using System.Threading.Tasks;
using Xunit;
using PonyLigaAPI.Controllers;
using PonyLigaAPI.Models;

namespace PonyLigaAPI.Tests
{
    [Collection("Sequential")]
    public class TeamPonyControllerUnitTest
    {
        [Fact]
        public async Task TestGetTeamPoniesAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamPoniesController(dbContext);

            var response = await controller.GetTeamPonies();

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.NotEmpty(value);
        }

        [Fact]
        public async Task TestGetTeamPoniesAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamPoniesController(dbContext);

            _ = controller.DeleteTeamPony(1, 1);
            var response = await controller.GetTeamPonies();

            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestGetTeamPonyAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamPoniesController(dbContext);

            var response = await controller.GetTeamPony(1,1);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, response.Value.ponyId);
            Assert.Equal(1, response.Value.teamId);
        }

        [Fact]
        public async Task TestGetTeamPonyAsyncFalse()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamPoniesController(dbContext);

            var response = await controller.GetTeamPony(1,2);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestPostTeamPonyAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamPoniesController(dbContext);

            var teamPony = new TeamPony
            {
                ponyId = 2,
                teamId = 1
            };
            teamPony.pony = null;
            teamPony.team = null;

            var response = await controller.PostTeamPony(teamPony);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.CreatedAtActionResult", code);
        }

        [Fact]
        public async Task TestPostTeamPonyAsyncConflict()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamPoniesController(dbContext);

            var teamPony = new TeamPony
            {
                ponyId = 1,
                teamId = 1
            };
            teamPony.pony = null;
            teamPony.team = null;

            var response = await controller.PostTeamPony(teamPony);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.ConflictResult", code);
        }

        [Fact]
        public async Task TestDeleteTeamPonyAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamPoniesController(dbContext);

            var response = await controller.DeleteTeamPony(1,2);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);

        }
        [Fact]
        public async Task TestDeleteTeamPonyAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamPoniesController(dbContext);

            var response = await controller.DeleteTeamPony(1,1);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, response.Value.ponyId);
            Assert.Equal(1, response.Value.teamId);
        }
    }
}
