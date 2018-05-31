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
using IdentityServer4.Stores;
using IdentityServer4.Models;

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
                var claims = new List<Claim>();
                claims.Add(new Claim(JwtClaimTypes.Id, userRet.Data.Id));
                claims.Add(new Claim(JwtClaimTypes.Name, userRet.Data.Name));
                claims.Add(new Claim(JwtClaimTypes.Role, userRet.Data.Role.ToString()));
                
                //var claims = new List<Claim>() { new Claim("role", "admin") }; //根据 user 对象，设置不同的 role
                context.Result = new GrantValidationResult(userRet.Data.Id, OidcConstants.AuthenticationMethods.Password, claims);
            }

            return Task.CompletedTask;
        }
    }

    //动态查询Client
    //public class CustomClientStore : IClientStore
    //{
    //    public Task<Client> FindClientByIdAsync(string clientId)
    //    {

    //        var result = _context.Clients.Where(x => x.ClientId == clientId).FirstOrDefault();

    //        return Task.FromResult(result.ToModel());
    //    }
    //}
}
