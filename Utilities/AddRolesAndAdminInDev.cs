using Microsoft.AspNetCore.Identity;
using Store444.Dopomoga;
using Store444.Models;

namespace Store444.Utilities;

public static class AddRolesAndAdminInDev
{
    public static async Task AddToRolesAsync(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var name in AllRoles.roles)
        {
            if (!await _roleManager.RoleExistsAsync(name))
            {
                var role = new IdentityRole { Name = name };
                await _roleManager.CreateAsync(role);
            }
        }
    }

    public static async Task AddToUsersAsync(this WebApplication app, IConfiguration configuration)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var _userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        var adminUser = await _userManager.FindByEmailAsync(configuration["Username"]);
        if (adminUser is null)
        {
            adminUser = new User()
            {
                UserName = configuration["Username"],
                Email = configuration["Email"],
                FirstName = configuration["FirstName"],
                SurName = configuration["SurName"],
                PhoneNumber = configuration["Phone"]
            };
            await _userManager.CreateAsync(adminUser, configuration["Password"]);
        }

        if (!await _userManager.IsEmailConfirmedAsync(adminUser))
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(adminUser);
            await _userManager.ConfirmEmailAsync(adminUser, token);
            await _userManager.AddToRoleAsync(adminUser, AllRoles.Admin);
        }
    }
}
