using Microsoft.EntityFrameworkCore;
using PonyLigaAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PonyLigaAPI.Tests
{
    public static class DbContextMocker
    {
        public static PonyLigaAPIContext GetPonyLigaAPIContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PonyLigaAPIContext>()
                .UseInMemoryDatabase("PonyLigaAPITest")
                .Options;

            var dbContext = new PonyLigaAPIContext(options);

            dbContext.Seed();

            return dbContext;
        }
    }
}
