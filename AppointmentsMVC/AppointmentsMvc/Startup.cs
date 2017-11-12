using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AppointmentsMvc.Startup))]
namespace AppointmentsMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
