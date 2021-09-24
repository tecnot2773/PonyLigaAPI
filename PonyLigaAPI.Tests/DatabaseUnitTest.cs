using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PonyLigaAPI.Tests
{
    [Collection("Sequential")]
    public class DatabaseUnitTest
    {
        [Fact]
        public void TestBuildConnectionString()
        {
            var envString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb");
            if (envString != null)
            {
                var connectionString = Models.PonyLigaAPIContext.BuildConnectionString();

                Assert.Matches("server=.*;port=.*;database=pony_liga;user=.*;password=.*", connectionString);
            }
        }
    }
}
