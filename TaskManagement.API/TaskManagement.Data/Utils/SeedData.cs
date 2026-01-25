using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Data.Utils
{
    public static class SeedData
    {
        public static async Task SeedRoles(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                List<IdentityRole> roles = new List<IdentityRole>
                {
                    new IdentityRole { Name = "DEVELOPER", NormalizedName = "DEVELOPER" },
                    new IdentityRole { Name = "MANAGER", NormalizedName = "MANAGER" },
                    new IdentityRole { Name = "SUPPORT", NormalizedName = "SUPPORT" },
                    new IdentityRole { Name = "TESTER", NormalizedName = "TESTER" },
                    new IdentityRole { Name = "ADMIN", NormalizedName = "ADMIN" }
                };

                foreach(var role in roles)
                {
                    if(!await roleManager.RoleExistsAsync(role.Name!))
                    {
                        await roleManager.CreateAsync(role);
                    }
                }
            }
        }
    }
}
