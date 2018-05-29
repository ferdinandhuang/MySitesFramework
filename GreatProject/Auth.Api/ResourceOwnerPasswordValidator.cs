using Auth.IRespositories;
using IdentityModel;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Auth.Api
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IUserRepositories userRepositories;
        public ResourceOwnerPasswordValidator(IUserRepositories _userRepositories)
        {
            userRepositories = _userRepositories;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var username = context.UserName;
            var passwors = context.Password;
            var userRet = userRepositories.Search(username, passwors);

            var users = userRepositories.GetByRedisCached();

            if (userRet.Status == Framework.Core.Common.Status.Success)
            {
                var claim = new List<Claim>();
                claim.Add(new Claim(JwtClaimTypes.Name, userRet.Data.Name));
                claim.Add(new Claim(JwtClaimTypes.Role, "admin"));
                context.Result = new GrantValidationResult("admin", OidcConstants.AuthenticationMethods.Password, claims: claim);
            }

            return Task.CompletedTask;
        }
    }
}
