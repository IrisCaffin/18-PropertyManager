using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PropertyManager.Api.Infrastructure
{
    public class PropertyManagerAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        // Going to override 2 methods of the OAuthAuthorizationServerProvider Class
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // The following method is being called by ASP.Net.Identity and we're overriding it
            // We're intercepting certain events in the OAuth dance; most things are already taken care of for us
            // But there are certain things that are our responsibility (overriding)

            // The below is allowing Cors. We already allowed Cors but we also have to allow it here
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
       
            // With the following method, we're going to validate the user
            // We're going to instantiate an authRepository 
            // We're wrapping it in a using statement, so its object (authRepository) is only live between these two curly braces
            using (var authRepository = new AuthorizationRepository())
            {
                // We're going to try and grab the user by calling that FindUser-method we just wrote
                // And we're going to pass in the username and password
                var user = await authRepository.FindUser(context.UserName, context.Password);

                // if username/password don't match OR user doesn't exist
                if(user == null)
                {
                    // Set the error. Two parameters: type of error (invalid_grant) and incorrect username/password
                    context.SetError("invalid_grant", "The username or password is incorrect");

                    // Then return (don't continue at this point)
                    return;
                }

                // If we do find the user, we have to create a token
                else
                {
                    var token = new ClaimsIdentity(context.Options.AuthenticationType);
                    token.AddClaim(new Claim("sub", context.UserName));
                    token.AddClaim(new Claim("role", "user"));

                    // We're letting ASP.Net.Identity know we're happy with the login by validating the current context, passing in the token
                    // Once we validate the context, the token will be returned to the user
                    context.Validated(token);
                }
            }

             
        }
    }
}