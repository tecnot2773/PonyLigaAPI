using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using PonyLigaAPI.Models;
using System.Text.RegularExpressions;

namespace PonyLigaAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PonyLigaAPIContext>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public static string buildConnectionString()
        {
            // "server=localhost;database=library;user=mysqlschema;password=mypassword"
            string connectionString = "";
            string envConnectionString = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb").ToString();
            string regex = @".*=(\w*);.*=(.*):(.*);.*=(.*);.*=(.*)";

            Regex rg = new Regex(regex);
            MatchCollection matches = rg.Matches(envConnectionString);

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                string database = groups[1].ToString();
                string server = groups[2].ToString();
                string port = groups[3].ToString();
                string user = groups[4].ToString();
                string password = groups[5].ToString();

                connectionString = "server=" + server + ";userid=" + user + ";password=" + password + ";database=" + database + ";port=" + port;
                connectionString = "server=" + server + ";port=" + port + ";database=pony_liga;user=" + user + ";password=" + password;
            }


            return connectionString;
        }
    }
}
