using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Infraestructure.Identity.Models;

namespace Tickets.Infraestructure.Identity.Extensions
{
    public static class ModelBuilderExtension
    {
        private const string AdministratorRole = "ADMINISTRATOR";
        private const string BasicRole = "BASIC";
        private const string DefaultPassword = "P2$$w0rd";
        private const string adminEmail = "superadmin@gmail.com";
        private const string userEmail = "jesuscampos670@gmail.com";

        public static void Seed(this ModelBuilder modelBuilder)
        {
            List<IdentityRole> roles = new List<IdentityRole>()
           {
               new IdentityRole{Name = AdministratorRole, NormalizedName = AdministratorRole},
               new IdentityRole {Name =BasicRole, NormalizedName = BasicRole}
           };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser{ Email = userEmail, NormalizedEmail = userEmail},
                new ApplicationUser{ Email = adminEmail, NormalizedEmail = adminEmail}
            };

            modelBuilder.Entity<ApplicationUser>().HasData(users);

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            users[0].PasswordHash = passwordHasher.HashPassword(users[0], DefaultPassword);
            users[1].PasswordHash = passwordHasher.HashPassword(users[1], DefaultPassword);

            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();
            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = users[0].Id,
                RoleId = roles.FirstOrDefault(x => x.Name == BasicRole).Id
            });
            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = users[1].Id,
                RoleId = roles.FirstOrDefault(x => x.Name == BasicRole).Id
            });
            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = users[1].Id,
                RoleId = roles.FirstOrDefault(x => x.Name == AdministratorRole).Id
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        }
    }
}
