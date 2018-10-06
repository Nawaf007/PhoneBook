using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhoneBookApp1.Startup))]
namespace PhoneBookApp1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
