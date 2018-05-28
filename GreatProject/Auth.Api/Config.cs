﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
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
                new ApiResource("api")
            };
        }

        //Client
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId="client",
                    AccessTokenLifetime = 180,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = 1800,
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets={
                        new Secret("secret".Sha256())
                    },
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AllowedScopes={
                        "api"
                    },
                },
                new Client()
                {
                    ClientId = "DangguiSite",
                    AccessTokenLifetime = 180,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = 1800,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AlwaysSendClientClaims = true,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AllowedScopes =
                    {
                        "api"
                    },
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowOfflineAccess = true
                },
            };
        }

        //TestUser
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>{
                new TestUser{
                    SubjectId="1",
                    Username="wyt",
                    Password="123456",
                    Claims = new List<Claim>()
                    {
                        new Claim("wawa", "aoao"),
                    }
                }
            };
        }
    }
}
