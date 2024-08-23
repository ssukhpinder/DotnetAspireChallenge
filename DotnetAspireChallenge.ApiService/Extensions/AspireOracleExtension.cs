using DotnetAspireChallenge.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DotnetAspireChallenge.ApiService.Extensions
{
    public static class AspireOracleExtension
    {
        public static void MapOracleAspireEndpoint(this WebApplication app)
        {
            app.MapGet("/oracle", async (OracleDbContext oracleDbContext) =>
            {
                await oracleDbContext.CustomersPgsql.AddAsync(new Customer()
                {
                    Title = "test@gmail.com",
                    Description = "sukh"
                });
                int rows = await oracleDbContext.SaveChangesAsync();
                if (rows > 0)
                {
                    return await oracleDbContext.CustomersPgsql.FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            });

            //if (app.Environment.IsDevelopment())
            //{
            //    using (var scope = app.Services.CreateScope())
            //    {
            //        var context = scope.ServiceProvider.GetRequiredService<OracleDbContext>();
            //        context.Database.EnsureCreated();
            //    }
            //}
        }
    }


    internal class OracleDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Customer> CustomersPgsql => Set<Customer>();
    }

}
