using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Journal.Startup))]
namespace Journal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
