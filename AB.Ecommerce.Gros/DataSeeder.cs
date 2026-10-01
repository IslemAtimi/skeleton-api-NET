using System;
using AB.Ecommerce.Gros.Business;
using AB.Ecommerce.Gros.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AB.Ecommerce.Gros
{
    public class DataSeeder
    {



        static Dictionary<string, string[]> RolePermissions = new Dictionary<string, string[]> {

            //{ Roles.Client,  new string[] {
            //        Permissions.Action1
            //    }
            //},

            { SystemRoles.Admin,  new string[] { } },
            { SystemRoles.Manager,  new string[] { } },
            { Roles.Customer,  new string[] { } }
        };

        static Dictionary<string, string[]> UserRoles = new Dictionary<string, string[]> {
            { "admin", new string[] {
                    SystemRoles.Admin
                }
            },
            {
                "manager", new string[] {
                    SystemRoles.Manager
                }
            }
        };

        static string DefaultPassword = "admin@artp@2026";
        static string DefaultEmail = "admin@aziz.dz";

        static string ManagerPassword = "manager@2026";
        static string ManagerEmail = "manager@artp.dz";



        public static async Task SeedDataAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();


                await SeedRolesAsync(_roleManager);
                await SeedUsersAsync(_userManager);
            }

        }


        private static async Task SeedRolesAsync(RoleManager<IdentityRole> _roleManager)
        {

            foreach (var rolePermission in RolePermissions)
            {
                var role = await _roleManager.FindByNameAsync(rolePermission.Key);
                var created = true;
                if (role == null)
                {
                    created = false;
                    var res = await _roleManager.CreateAsync(new IdentityRole(rolePermission.Key));
                    
                    created = res.Succeeded;
                }

                if (created)
                {
                    role = await _roleManager.FindByNameAsync(rolePermission.Key);
                    var claims = await _roleManager.GetClaimsAsync(role);
                    claims = claims.Where(c => c.Type == "Permission").ToList();
                    foreach (var permission in rolePermission.Value)
                    {
                        if (!claims.Any(c => c.Value == permission))
                        {
                            await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim("Permission", permission));
                        }
                    }
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<IdentityUser> _userManager)
        {

            foreach (var userRole in UserRoles)
            {
                var user = await _userManager.FindByNameAsync(userRole.Key);
                var created = true;
                if (user == null)
                {
                    created = false;
                    user = new IdentityUser(userRole.Key);
                    user.Email = userRole.Value[0] == SystemRoles.Manager ? ManagerEmail : DefaultEmail;
                    var res = await _userManager.CreateAsync(user);
                    await _userManager.AddPasswordAsync(user, userRole.Value[0]== SystemRoles.Manager?ManagerPassword:DefaultPassword);
                    created = res.Succeeded;
                }

                if (created)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in userRole.Value)
                    {
                        if (roles.Contains(role))
                        {
                            break;
                        }
                        await _userManager.AddToRoleAsync(user, role);
                    } 
                }
            }
        }

    }
}

