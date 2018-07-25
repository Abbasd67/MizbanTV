using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MizbanTV.Startup))]
namespace MizbanTV
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
