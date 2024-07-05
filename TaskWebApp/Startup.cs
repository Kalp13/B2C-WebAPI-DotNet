using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TaskWebApp.Startup))]

namespace TaskWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            IdentityModelEventSource.ShowPII = true;

            ConfigureAuth(app);
        }
    }
}
