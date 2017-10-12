using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MLY.Startup))]
namespace MLY
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
