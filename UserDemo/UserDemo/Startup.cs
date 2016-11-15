using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UserDemo.Startup))]
namespace UserDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
