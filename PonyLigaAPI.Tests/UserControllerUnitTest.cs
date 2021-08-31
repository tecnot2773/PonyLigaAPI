using System;
using System.Threading.Tasks;
using Xunit;
using PonyLigaAPI.Controllers;
using PonyLigaAPI.Models;

namespace PonyLigaAPI.Tests
{
    [Collection("Sequential")]
    public class UserControllerUnitTest
    {
        [Fact]
        public async Task TestGetUsersAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new UserController(dbContext);

            var response = await controller.GetUser();

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.NotEmpty(value);
        }

        [Fact]
        public async Task TestGetUserAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new UserController(dbContext);

            var response = await controller.GetUser(1);

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.NotNull(value);
        }

        [Fact]
        public async Task TestGetUserAsyncFalse()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new UserController(dbContext);

            var response = await controller.GetUser(2);

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Null(value);
        }


        [Fact]
        public async Task TestPutUserAsyncBadRequest()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new UserController(dbContext);

            var id = 2;
            var newName = "New Name";
            var user = await controller.GetUser(1);

            user.Value.firstName = newName;
            
            var response = await controller.PutUser(id, user.Value);
            var code = response.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.BadRequestResult", code);
        }

        [Fact]
        public async Task TestPutUserAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new UserController(dbContext);

            var user = new User
            {
                id = 2,
                firstName = "Test",
                surName = "Test",
                loginName = "Test",
                passwordHash = "123123123"
            };

            var response = await controller.PutUser(2, user);
            var code = response.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestPutUserAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new UserController(dbContext);

            var id = 1;
            var newName = "New Name";
            var user = await controller.GetUser(id);

            user.Value.firstName = newName;

            var response = await controller.PutUser(id, user.Value);

            user = await controller.GetUser(id);



            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(newName, user.Value.firstName);
        }

        [Fact]
        public async Task TestPostUserAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new UserController(dbContext);

            var user = new User
            {
                firstName = "Test",
                surName = "Test",
                loginName = "Test",
                passwordHash = "123123123"
            };

            var response = await controller.PostUser(user);

            var newUser = await controller.GetUser(2);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(2, newUser.Value.id);
        }

        [Fact]
        public async Task TestDeleteUserAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new UserController(dbContext);

            var response = await controller.DeleteUser(1);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, response.Value.id);
        }

        [Fact]
        public async Task TestDeleteUserAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new UserController(dbContext);

            var response = await controller.DeleteUser(2);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestUserLoginAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new UserController(dbContext);

            var user = new User
            {
                loginName = "Test",
                passwordHash = "123123123"
            };

            var response = await controller.LoginUser(user);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, response.Value.id);
        }

        [Fact]
        public async Task TestUserLoginWrongPasswordAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new UserController(dbContext);

            var user = new User
            {
                loginName = "Test",
                passwordHash = "wrongpassword"
            };

            var response = await controller.LoginUser(user);
            var code = response.Result.ToString();
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.UnauthorizedResult",code);
        }

        [Fact]
        public async Task TestUserLoginWrongLoginNamedAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new UserController(dbContext);

            var user = new User
            {
                loginName = "Wrong name",
                passwordHash = "123123123"
            };

            var response = await controller.LoginUser(user);
            var code = response.Result.ToString();
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.UnauthorizedResult", code);
        }

    }
}
