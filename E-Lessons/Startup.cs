using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(E_Lessons.Startup))]
namespace E_Lessons
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
