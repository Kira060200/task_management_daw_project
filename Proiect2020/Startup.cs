﻿using Microsoft.Owin;
using Owin;
using Proiect2020.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
[assembly: OwinStartupAttribute(typeof(Proiect2020.Startup))]
namespace Proiect2020
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Se apeleaza o metoda in care se adauga contul de administrator si rolurile aplicatiei
            CreateAdminUserAndApplicationRoles();
        }

        private void CreateAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("Admin"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "admin@gmail.com";
                user.Email = "admin@gmail.com";
                var adminCreated = UserManager.Create(user, "!1Admin");

                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("Organizator"))
            {
                var role = new IdentityRole();
                role.Name = "Organizator";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Membru"))
            {
                var role = new IdentityRole();
                role.Name = "Membru";
                roleManager.Create(role);
            }
        }
    }
}
