using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SNCRegistration.Startup))]
namespace SNCRegistration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
