using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Jewerly.Web.Startup))]
namespace Jewerly.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
