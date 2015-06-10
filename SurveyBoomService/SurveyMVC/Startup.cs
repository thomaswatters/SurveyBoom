using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SurveyMVC.Startup))]
namespace SurveyMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
