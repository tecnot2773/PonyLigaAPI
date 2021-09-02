using System;
using System.Threading.Tasks;
using Xunit;
using PonyLigaAPI.Controllers;
using PonyLigaAPI.Models;

namespace PonyLigaAPI.Tests
{
    [Collection("Sequential")]
    public class PonyControllerUnitTest
    {
        [Fact]
        public async Task TestGetPoniesAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new PonyController(dbContext);

            var response = await controller.GetPonies();

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.NotEmpty(value);
        }

        [Fact]
        public async Task TestGetPoniesAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new PonyController(dbContext);

            _ = controller.DeletePony(1);
            var response = await controller.GetPonies();

            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestGetTeamAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new PonyController(dbContext);

            var response = await controller.GetPony(1);

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, value.id);
        }

        [Fact]
        public async Task TestGetPonyAsyncFalse()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new PonyController(dbContext);

            var response = await controller.GetPony(2);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestPutPonyAsyncBadRequest()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new PonyController(dbContext);

            var id = 2;
            var newName = "New Name";
            var user = await controller.GetPony(1);

            user.Value.name = newName;

            var response = await controller.PutPony(id, user.Value);
            var code = response.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.BadRequestResult", code);
        }

        [Fact]
        public async Task TestPutPonyAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new PonyController(dbContext);

            var pony = new Pony
            {
                id = 2,
                race = "Gaul",
                name = "Hans",
                age = "12"
            };

            var response = await controller.PutPony(2, pony);
            var code = response.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestPutPonyAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new PonyController(dbContext);

            var id = 1;
            var newName = "New Name";
            var team = await controller.GetPony(id);

            team.Value.name = newName;

            var response = await controller.PutPony(id, team.Value);

            team = await controller.GetPony(id);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(newName, team.Value.name);
        }

        [Fact]
        public async Task TestPostPonyAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new PonyController(dbContext);

            var pony = new Pony
            {
                race = "Gaul",
                name = "Hans",
                age = "12"
            };
            pony.teamPonies = null;
            pony.teamId = 1;
            pony.teams = null;

            var response = await controller.PostPony(pony);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.CreatedAtActionResult", code);
        }

        [Fact]
        public async Task TestDeletePonyAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new PonyController(dbContext);

            var response = await controller.DeletePony(2);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);

        }
        [Fact]
        public async Task TestDeletePonyAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new PonyController(dbContext);

            var response = await controller.DeletePony(1);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, response.Value.id);
        }
    }
}
