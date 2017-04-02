using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WorkFlowaMig.Models;

[assembly: OwinStartupAttribute(typeof(WorkFlowaMig.Startup))]
namespace WorkFlowaMig
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUser();
        }
        private void CreateRolesAndUser()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //create admin role if not exist
            if(!roleManager.RoleExists("admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "admin";
                roleManager.Create(role);

                //create user in role 
                var user = new ApplicationUser();
                user.UserName = "Nader";
                string pwd = "Na22491703@";
                var chkuser = userManager.Create(user,pwd);
                if(chkuser.Succeeded)
                {
                    var rslt = userManager.AddToRole(user.Id, "admin");
                }
            }
            if (!roleManager.RoleExists("manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "manager";
                roleManager.Create(role);

            
            }
        }
    }
}
