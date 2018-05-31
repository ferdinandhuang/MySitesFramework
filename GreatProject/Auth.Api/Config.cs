using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Auth.Api
{
    public class Config
    {
        //ApiResource
        public static IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>
            {
                //new ApiResource("Service", "My Service"){
                //    UserClaims = new List<string>(){
                //        JwtClaimTypes.Id,
                //        JwtClaimTypes.Name,
                //        JwtClaimTypes.Role,
                //    },
                //},
                new ApiResource("MainSite", "Main Site")
                {
                    UserClaims = new List<string>(){
                        JwtClaimTypes.Id,
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Role,
                    },
                    ApiSecrets = {
                        new Secret("secret".Sha256()),
                    }
                },
            };
        }

        //Client
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "DangguiSite",
                    AccessTokenLifetime = 600,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = 600,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RedirectUris ={
                        "http://localhost:5001/",
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AlwaysSendClientClaims = true,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AllowedScopes =
                    {
                        "MainSite",
                        IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                    AccessTokenType = AccessTokenType.Reference,
                    AllowOfflineAccess = true
                },
            };
        }
    }
}
