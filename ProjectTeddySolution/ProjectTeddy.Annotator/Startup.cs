using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectTeddy.Annotator.Startup))]
namespace ProjectTeddy.Annotator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
