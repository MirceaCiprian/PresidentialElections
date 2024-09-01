using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp1.Controllers;
using WebApp1.Models;

namespace WebApp1.Data
{
    public class ContextSeed
    {

        public static async Task SeedRolesAsync(UserManager<MyUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Normal.ToString()));
        }
    }

}
