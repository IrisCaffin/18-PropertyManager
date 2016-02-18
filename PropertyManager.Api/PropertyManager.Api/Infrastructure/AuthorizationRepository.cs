using Microsoft.AspNet.Identity;
using PropertyManager.Api.Domain;
using PropertyManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PropertyManager.Api.Infrastructure
{
    public class AuthorizationRepository : IDisposable
    {
        private UserManager<PropertyManagerUser> _userManager;

        public async Task<IdentityResult> RegisterUser(RegistrationModel model)
        {
            var propertyManagerUser = new PropertyManagerUser
            {
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(propertyManagerUser, model.Password);

            return result;
        }

        public void Dispose()
        {
            _userManager.Dispose();
        }
    }
}