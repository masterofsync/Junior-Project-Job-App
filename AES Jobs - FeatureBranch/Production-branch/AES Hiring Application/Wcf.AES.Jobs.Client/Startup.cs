using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WcfAESJobs.Client.Startup))]
namespace WcfAESJobs.Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
