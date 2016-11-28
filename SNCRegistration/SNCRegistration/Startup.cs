using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SNCRegistration.ViewModels;

[assembly: OwinStartupAttribute(typeof(SNCRegistration.Startup))]
namespace SNCRegistration
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        //From C Sharp Tut. Creates initial System Admin and Roles
        private void createRolesandUsers() {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     



            // creating Creating Manager role     
            if (!roleManager.RoleExists("SystemAdmin")) {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "SystemAdmin";
                roleManager.Create(role);

            }

            // creating Creating Manager role     
            if (!roleManager.RoleExists("FullAdmin")) {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "FullAdmin";
                roleManager.Create(role);

            }

            // creating Creating Employee role     
            if (!roleManager.RoleExists("VolunteerAdmin")) {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "VolunteerAdmin";
                roleManager.Create(role);

            }

            if (!roleManager.RoleExists("UnassignedUser")) {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "UnassignedUser";
                roleManager.Create(role);

            }

            if (UserManager.FindByName("SNCRAdmin") == null) {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "SystemAdmin";
                roleManager.Create(role);
                var user = new ApplicationUser();
                user.UserName = "SNCRAdmin";
                user.Email = "SNCRAdmin@LFL.com";

                string userPWD = "LFL1234!";
                var chkUser = UserManager.Create(user, userPWD);
                UserManager.AddToRole(user.Id, "SystemAdmin");

            }
        }
    }
}
