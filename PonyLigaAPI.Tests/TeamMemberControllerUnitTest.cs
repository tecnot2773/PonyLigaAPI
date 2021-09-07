using System;
using System.Threading.Tasks;
using Xunit;
using PonyLigaAPI.Controllers;
using PonyLigaAPI.Models;

namespace PonyLigaAPI.Tests
{
    [Collection("Sequential")]
    public class TeamMemberControllerUnitTest
    {
        [Fact]
        public async Task TestGetTeamMembersAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamMemberController(dbContext);

            var response = await controller.GetTeamMembers();

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.NotEmpty(value);
        }

        [Fact]
        public async Task TestGetTeamMembersAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamMemberController(dbContext);

            _ = controller.DeleteTeamMember(1);
            var response = await controller.GetTeamMembers();

            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestGetTeamMemberAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamMemberController(dbContext);

            var response = await controller.GetTeamMember(1);

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, value.id);
        }

        [Fact]
        public async Task TestGetTeamMemberAsyncFalse()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamMemberController(dbContext);

            var response = await controller.GetTeamMember(2);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestPutTeamMemberAsyncBadRequest()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamMemberController(dbContext);

            var id = 2;
            var newFirstName = "Mike";
            var teamMember = await controller.GetTeamMember(1);

            teamMember.Value.firstName = newFirstName;

            var response = await controller.PutTeamMember(id, teamMember.Value);
            var code = response.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.BadRequestResult", code);
        }

        [Fact]
        public async Task TestPutTeamMemberAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamMemberController(dbContext);

            var teamMember = new TeamMember
            {
                id = 2,
                firstName = "Peter",
                surName = "Müller",
                teamId = 1
            };

            var response = await controller.PutTeamMember(2, teamMember);
            var code = response.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }

        [Fact]
        public async Task TestPutTeamMemberAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamMemberController(dbContext);

            var id = 1;
            var newFirstName = "Lars";
            var team = await controller.GetTeamMember(id);

            team.Value.firstName = newFirstName;

            var response = await controller.PutTeamMember(id, team.Value);

            team = await controller.GetTeamMember(id);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(newFirstName, team.Value.firstName);
        }

        [Fact]
        public async Task TestPostTeamMemberAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamMemberController(dbContext);

            var teamMember = new TeamMember
            {
                firstName = "Peter",
                surName = "Müller",
                teamId = 1
            };

            teamMember.team = null;

            var response = await controller.PostTeamMember(teamMember);

            var newResult = await controller.GetTeamMember(2);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(2, newResult.Value.id);
        }

        [Fact]
        public async Task TestDeleteTeamMemberAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamMemberController(dbContext);

            var response = await controller.DeleteTeamMember(2);
            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);

        }

        [Fact]
        public async Task TestDeleteTeamMemberAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamMemberController(dbContext);

            var response = await controller.DeleteTeamMember(1);

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal(1, response.Value.id);
        }

        [Fact]
        public async Task TestGetTeamMembersByTeamIdAsync()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamMemberController(dbContext);

            var response = await controller.GetTeamMembersByTeamId(1);

            var value = response.Value;

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.NotEmpty(value);
        }

        [Fact]
        public async Task TestGetTeamMembersByTeamIdAsyncNotFound()
        {
            var dbContext = DbContextMocker.GetPonyLigaAPIContext(Guid.NewGuid().ToString());
            var controller = new TeamMemberController(dbContext);

            _ = controller.DeleteTeamMember(1);
            var response = await controller.GetTeamMembersByTeamId(1);

            var code = response.Result.ToString();

            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();

            Assert.Equal("Microsoft.AspNetCore.Mvc.NotFoundResult", code);
        }
    }
}
