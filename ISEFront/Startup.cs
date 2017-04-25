using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISEFront.Startup))]
namespace ISEFront
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
