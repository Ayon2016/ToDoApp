using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IS8012.AyonChoudhury.LabProject.Startup))]
namespace IS8012.AyonChoudhury.LabProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
