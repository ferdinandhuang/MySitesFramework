using Auth.IRespositories;
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

            if(userRet.Status == Framework.Core.Common.Status.Success)
            {
                var claim = new List<Claim>();
                claim.Add(new Claim("Name", userRet.Data.Name));
                context.Result = new GrantValidationResult(subject: "admin", authenticationMethod: "custom", claims: claim);
            }

            return Task.CompletedTask;
        }
    }
}
