using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

// Adding an attribute to our namespace
// We're telling Owin where to find the configuration file to start this application
// I.e. we're telling Owin to use PropertyManager.Api.Startup to boot up everything
[assembly: OwinStartup(typeof(PropertyManager.Api.Startup))]
namespace PropertyManager.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Call the Registration method located in WebApiConfig.cs by instantiating its HttpConfiguration parameters
            HttpConfiguration config = new HttpConfiguration();
            // Call WebApiConfig.Register, passing in the instance of HttpConfiguration (config) just made above
            WebApiConfig.Register(config);

            // Tell app to use Cors 
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);

            // Now that the above is there, we can go ahead and delete Global.asax file, it's no longer needed
        }
    }
}