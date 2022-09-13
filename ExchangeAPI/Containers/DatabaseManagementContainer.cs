using ExchangeAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace ExchangeAPI.Containers;

public static class DatabaseManagementContainer
{
    public static void Migrate(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            serviceScope.ServiceProvider.GetService<Exchange_DBContext>()!.Database.EnsureCreated();
        }
    }
}