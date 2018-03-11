using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LMYFrameWorkMVC.Web.Startup))]
namespace LMYFrameWorkMVC.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
            //for signalr to save the data on sql server
            //string sqlConnectionString = "Connecton string to your SQL DB";
    		//GlobalHost.DependencyResolver.UseSqlServer(sqlConnectionString);
            
            //signal r mapping
            app.MapSignalR();
        }
    }
}
