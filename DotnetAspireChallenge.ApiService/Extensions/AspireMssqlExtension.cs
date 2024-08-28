using DotnetAspireChallenge.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DotnetAspireChallenge.ApiService.Extensions
{
    public static class AspireMssqlExtension
    {
        public static void MapMssqlAspireEndpoint(this WebApplication app)
        {
            app.MapGet("/mssql", async (MssqlDbContext mssqlDbContext) =>
            {
                await mssqlDbContext.Customers.AddAsync(new Customer()
                {
                    Title = "test@gmail.com",
                    Description = "sukh"
                });
                int rows = await mssqlDbContext.SaveChangesAsync();
                if (rows > 0)
                {
                    return await mssqlDbContext.Customers.FirstOrDefaultAsync();
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
            //        var context = scope.ServiceProvider.GetRequiredService<MssqlDbContext>();
            //        context.Database.EnsureCreated();
            //    }
            //}
        }
    }

    internal class MssqlDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Customer> Customers => Set<Customer>();
    }
}
