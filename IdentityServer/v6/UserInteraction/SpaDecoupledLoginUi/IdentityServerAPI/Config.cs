// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
using System.Collections.Generic;

namespace IdentityServerAPI.Resources
{
    public static class Config
    {
        public static string identityWebUIHost = "https://localhost:5004";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                }
            }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
            new ApiScope("api1", "MyAPI")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
            // JavaScript Client
            new Client
            {
                ClientId = "js",
                ClientName = "JavaScript Client",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,

                RedirectUris =           { "https://localhost:5003/callback.html" },
                PostLogoutRedirectUris = { "https://localhost:5003/index.html" },
                AllowedCorsOrigins =     { "https://localhost:5003" },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "api1"
                }
            }
            };
    }
}