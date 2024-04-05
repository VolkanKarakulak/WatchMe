using System;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WatchMe.Models;
using WatchMe.Identity.Data;


namespace WatchMe.Data
{
    public class DBInitializer
    {
        public DBInitializer(WatchMeContext? context, RoleManager<AppRole>? roleManager, UserManager<AppUser>? userManager)
        {
           
            AppRole appRole;
            AppUser appUser;
            //Brand? brand = null;

            if (context != null)
            {

                context.SaveChanges();
                if (roleManager != null)
                {
                    if (roleManager.Roles.Count() == 0)
                    {
                        appRole = new AppRole("Admin");
                        roleManager.CreateAsync(appRole).Wait();
                        appRole = new AppRole("BrandAdmin");
                        roleManager.CreateAsync(appRole).Wait();
                        appRole = new AppRole("RestaurantAdmin");
                        roleManager.CreateAsync(appRole).Wait();
                    }
                }
                if (userManager != null)
                {
                    if (userManager.Users.Count() == 0)
                    {
                            appUser = new AppUser();
                            appUser.UserName = "Admin";
                            appUser.BirthDate = DateTime.Today;
                            appUser.Passive = false;
                            appUser.Name = "Admin";
                            appUser.Email = "abc@def.com";
                            appUser.PhoneNumber = "1112223344";
                            userManager.CreateAsync(appUser, "Admin123!").Wait();
                            userManager.AddToRoleAsync(appUser, "Admin").Wait();

                            appUser = new AppUser();
                            appUser.UserName = "CustomerRepsentative";
                            appUser.BirthDate = DateTime.Today;
                            appUser.Passive = false;
                            appUser.Name = "CustomerRepsentative";
                            appUser.Email = "abc@def.com";
                            appUser.PhoneNumber = "1112223344";
                            userManager.CreateAsync(appUser, "CustomerRep123!").Wait();
                            userManager.AddToRoleAsync(appUser, "CustomerRepsentative").Wait();

                            appUser = new AppUser();
                            appUser.UserName = "ContentAdmin";
                            appUser.BirthDate = DateTime.Today;
                            appUser.Passive = false;
                            appUser.Name = "ContentAdmin";
                            appUser.Email = "abc@def.com";
                            appUser.PhoneNumber = "1112223344";
                            userManager.CreateAsync(appUser, "Content123!").Wait();
                            userManager.AddToRoleAsync(appUser, "ContentAdmin").Wait();

                    }
                }
            }
        }
    }
}