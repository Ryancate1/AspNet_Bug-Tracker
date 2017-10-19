using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(rcate_BugTracker.Startup))]
namespace rcate_BugTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
