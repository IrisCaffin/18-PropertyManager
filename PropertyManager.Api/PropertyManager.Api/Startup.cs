using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PropertyManager.Api.Infrastructure;
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

            // Call method
            ConfigureOAuth(app);

            // Tell app to use Cors 
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);

            // Now that the above is there, we can go ahead and delete Global.asax file, it's no longer needed
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            // Configure authentication
            // The object below represents options for authentication
            // The default options are good for now so we're not going to change anything
            var authenticationOptions = new OAuthBearerAuthenticationOptions();

            // Tell the app to use OAuthBearerAuthentication, passing in the authenticationOptions that we just instantiated
            app.UseOAuthBearerAuthentication(authenticationOptions);

            // Configure authorization
            // We do want to customize is how authorization works in our system
            // Same pattern as above, we're making options and then pass those options to the application
            var authorizationOptions = new OAuthAuthorizationServerOptions
            {
                // We are going to configure 4 properties here to customize authorization

                // We don't have https set up (secure set up)
                // So just for testing purposes, we're going to allow insecure http
                AllowInsecureHttp = true,

                // Because we're not writing a controller that accepts information (that's taken care of by ASP.Net.Identity)
                // We need to tell ASP.Net.Identity what route is; where is my user going to post to grab a token
                // We're telling it the endpoint path is a new path string and you have to hit /api/token to grab a token
                TokenEndpointPath = new PathString("/api/token"),

                // The token is only good for 1 day
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),

                // ASP.Net.Identity now wants to know where's the class that you wrote to intercept the events I'm going to throw at you
                Provider = new PropertyManagerAuthorizationServerProvider()
            };

            app.UseOAuthAuthorizationServer(authorizationOptions);
        }
    }
}