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
        }
    }
}
