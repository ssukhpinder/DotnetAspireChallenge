using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DotnetAspireChallenge.ApiService.Extensions
{
    public static class AspirePgsqlExtension
    {
        public static void MapPgsqlAspireEndpoint(this WebApplication app)
        {
            app.MapGet("/pgsql", async (PgsqlDbContext pgsqlDbContext) =>
            {
                await pgsqlDbContext.CustomersPgsql.AddAsync(new CustomerPgsql()
                {
                    Title = "test@gmail.com",
                    Description = "sukh"
                });
                int rows = await pgsqlDbContext.SaveChangesAsync();
                if (rows > 0)
                {
                    return await pgsqlDbContext.CustomersPgsql.FirstOrDefaultAsync();
                }
                else
                {
                    return null;
                }
            });

            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PgsqlDbContext>();
                    context.Database.EnsureCreated();
                }
            }
        }
    }


    internal class PgsqlDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<CustomerPgsql> CustomersPgsql => Set<CustomerPgsql>();
    }

    public class CustomerPgsql
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
