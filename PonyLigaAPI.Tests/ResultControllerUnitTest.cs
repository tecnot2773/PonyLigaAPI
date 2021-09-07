using System;
using System.Threading.Tasks;
using Xunit;
using PonyLigaAPI.Controllers;
using PonyLigaAPI.Models;

namespace PonyLigaAPI.Tests
{
    [Collection("Sequential")]
    public class ResultControllerUnitTest
    {
        [Fact]
        public async Task TestGetResultsAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new ResultController(dbContext);

            var response = await controller.GetResults();

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.NotEmpty(value);
        }

        [Fact]
        public async Task TestGetResultsAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new ResultController(dbContext);

            _ = controller.DeleteResult(1);
            var response = await controller.GetResults();

            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestGetResultAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new ResultController(dbContext);

            var response = await controller.GetResult(1);

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, value.id);
        }

        [Fact]
        public async Task TestGetResultAsyncFalse()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new ResultController(dbContext);

            var response = await controller.GetResult(2);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestGetResultSummaryAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new ResultController(dbContext);

            var response = await controller.GetResultSummary();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.NotEmpty(response.Value);
        }

        [Fact]
        public async Task TestGetResultSummaryAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new ResultController(dbContext);
            var teamController = new TeamController(dbContext);

            _ = await teamController.DeleteTeam(1);
            _ = await controller.DeleteResult(1);
            var response = await controller.GetResultSummary();

            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestPutResultAsyncBadRequest()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new ResultController(dbContext);

            var id = 2;
            var newGame = "Reiten";
            var result = await controller.GetResult(1);

            result.Value.game = newGame;

            var response = await controller.PutResult(id, result.Value);
            var code = response.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.BadRequestResult", code);
        }

        [Fact]
        public async Task TestPutResultAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new ResultController(dbContext);

            var result = new Result
            {
                id = 2,
                gameDate = new DateTime(),
                game = "Springen",
                time = "10:10:10.111",
                score = 10,
                teamId = 1
            };

            var response = await controller.PutResult(2, result);
            var code = response.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestPutResultAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new ResultController(dbContext);

            var id = 1;
            var newGame = "Reiten";
            var team = await controller.GetResult(id);

            team.Value.game = newGame;

            var response = await controller.PutResult(id, team.Value);

            team = await controller.GetResult(id);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(newGame, team.Value.game);
        }

        [Fact]
        public async Task TestPostResultAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new ResultController(dbContext);

            var result = new Result
            {
                gameDate = new DateTime(),
                game = "Springen",
                time = "10:10:10.111",
                score = 10,
                teamId = 1
            };

            result.team = null;

            var response = await controller.PostResult(result);

            var newResult = await controller.GetResult(2);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(2, newResult.Value.id);
        }

        [Fact]
        public async Task TestDeleteResultAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new ResultController(dbContext);

            var response = await controller.DeleteResult(2);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);

        }
        [Fact]
        public async Task TestDeleteResultAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new ResultController(dbContext);

            var response = await controller.DeleteResult(1);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, response.Value.id);
        }
    }
}
