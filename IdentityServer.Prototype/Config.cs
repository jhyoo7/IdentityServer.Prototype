using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer.Prototype
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "ee6c19b6-2cd5-477f-a5f3-9e3e5f4e16c4",
                    Username = "Jane",
                    Password = "1234",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Jane"),
                        new Claim("family_name", "Doe"),
                        new Claim("address", "123 main street"),
                        new Claim("role", "Public")
                    }
                },
                new TestUser
                {
                    SubjectId = "f3c2943c-c281-4ce7-849b-e0e30f991b25",
                    Username = "John",
                    Password = "1234",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "John"),
                        new Claim("family_name", "Doe"),
                        new Claim("address", "123 green street"),
                        new Claim("role", "Client")
                    }
                },
                new TestUser
                {
                    SubjectId = "0e16e6f3-d1af-4794-a65d-4e5dbf88f5a9",
                    Username = "Foo",
                    Password = "1234",
                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "Foo"),
                        new Claim("family_name", "Bar"),
                        new Claim("address", "123 blue street"),
                        new Claim("role", "Admin")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource("roles", "Your role(s)", new List<string>(){"role"})
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "Prototype Web",
                    ClientId = "prototypeWebClient",
                    AllowedGrantTypes = new List<string>{GrantType.Hybrid},
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44383/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44383/signout-callback-oidc"
                    }
                    //AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }
    }
}
