using System;
using System.Threading.Tasks;
using Xunit;
using PonyLigaAPI.Controllers;
using PonyLigaAPI.Models;

namespace PonyLigaAPI.Tests
{
    [Collection("Sequential")]
    public class GroupControllerUnitTest
    {
        [Fact]
        public async Task TestGetGroupsAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new GroupController(dbContext);

            var response = await controller.GetGroups();

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.NotEmpty(value);
        }

        [Fact]
        public async Task TestGetGroupsAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new GroupController(dbContext);

            _ = controller.DeleteGroup(1);
            var response = await controller.GetGroups();

            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestGetGroupAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new GroupController(dbContext);

            var response = await controller.GetGroup(1);

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, value.id);
        }

        [Fact]
        public async Task TestGetGroupAsyncFalse()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new GroupController(dbContext);

            var response = await controller.GetGroup(2);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestPutGroupAsyncBadRequest()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new GroupController(dbContext);

            var id = 2;
            var newName = "New Name";
            var user = await controller.GetGroup(1);

            user.Value.name = newName;

            var response = await controller.PutGroup(id, user.Value);
            var code = response.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.BadRequestResult", code);
        }

        [Fact]
        public async Task TestPutGroupAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new GroupController(dbContext);

            var group = new Group
            {
                id = 2,
                name = "PonyGroup",
                rule = 1,
                groupSize = 3,
                participants = "Merle, Jonas"
            };

            var response = await controller.PutGroup(2, group);
            var code = response.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestPutGroupAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new GroupController(dbContext);

            var id = 1;
            var newName = "New Name";
            var group = await controller.GetGroup(id);

            group.Value.name = newName;

            var response = await controller.PutGroup(id, group.Value);

            group = await controller.GetGroup(id);



            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(newName, group.Value.name);
        }

        [Fact]
        public async Task TestPostGroupAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new GroupController(dbContext);

            var group = new Group
            {
                name = "Test",
                rule = 2,
                groupSize = 4,
                participants = "Steffan, G?nter"
            };

            var response = await controller.PostGroup(group);

            var newGroup = await controller.GetGroup(2);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(2, newGroup.Value.id);
        }

        [Fact]
        public async Task TestDeleteGroupAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new GroupController(dbContext);

            var response = await controller.DeleteGroup(2);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestDeleteGroupAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new GroupController(dbContext);

            var response = await controller.DeleteGroup(1);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, response.Value.id);
        }
    }
}
