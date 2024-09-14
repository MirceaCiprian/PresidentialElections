﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WebApp1.Data;
using WebApp1.Models;

enum Roles
{
    Admin,
    User
}


namespace WebApp1.Controllers
{
    public class SeedData
    {
        public static void SeedDB(ApplicationDbContext context, string adminID)
        { 
            //if (context.Contact.Any())
            //{
            //    return;   // DB has been seeded
            //}

            //context.AddRange(
            //    new Contact
            //    {
            //        Name = "Debra Garcia",
            //        Address = "1234 Main St",
            //        City = "Redmond",
            //        State = "WA",
            //        Zip = "10999",
            //        Email = "debra@example.com",
            //        Status = ContactStatus.Approved,
            //        OwnerID = adminID
            //    });
        }

        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@contoso.com", "Admin", "Admin");
                await EnsureRole(serviceProvider, adminID, "Admin");

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@contoso.com", "Manager", "Manager");
                await EnsureRole(serviceProvider, managerID, "User");

                //SeedDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw,string Email, string FirstName, string LastName)
        {
            var userManager = serviceProvider.GetService<UserManager<MyUser>>();

            var user = await userManager.FindByNameAsync(Email);
            if (user == null)
            {
                user = new MyUser
                {
                    UserName = Email,
                    Email = Email,
                    FirstName = FirstName,
                    LastName = LastName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<MyUser>>();

            //if (userManager == null)
            //{
            //    throw new Exception("userManager is null");
            //}

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
    }
}
