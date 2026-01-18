using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Enums;
using ProductControl.Domain.Interfaces.Services;

namespace ProductControl.Infrastructure.Data.Context
{
    public class InitializeUser(IServiceProvider serviceProvider)
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PostgreDbContext>();
            var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

            if (await context.Users.AnyAsync(u => u.Role == Roles.administrator))
                return;

            var address = Address.Create("01001000", "Praça da Sé", "Sé", "São Paulo", "SP", "1", "").Value;

            var password = passwordService.GeneratePassword("Admin!123").Value;

            var admin = Users.Create(
                "Administrador do Sistema",
                "admin",
                password,
                Roles.administrator,
                address
            ).Value;

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}
