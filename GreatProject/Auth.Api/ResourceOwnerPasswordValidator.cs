using Auth.IRespositories;
using IdentityModel;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MySites.DataModels;
using Auth.IServices;

namespace Auth.Api
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IUserService userService;
        public ResourceOwnerPasswordValidator(IUserService _userService)
        {
            userService = _userService;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var username = context.UserName;
            var passwors = context.Password;
            var userRet = userService.SearchUserByPwd(username, passwors);

            if (userRet.Status == Framework.Core.Common.Status.Success)
            {
                var claim = new List<Claim>();
                claim.Add(new Claim(JwtClaimTypes.Name, userRet.Data.Name));
                claim.Add(new Claim(JwtClaimTypes.Role, userRet.Data.Role.ToString()));
                context.Result = new GrantValidationResult("admin", OidcConstants.AuthenticationMethods.Password, claims: claim);
            }

            return Task.CompletedTask;
        }
    }
}
