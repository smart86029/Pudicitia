using IdentityServer4;
using IdentityServer4.Models;

namespace Pudicitia.Identity.Api;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("enterprise"),
            new ApiScope("hr"),
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "enterprise.web",
                AllowedGrantTypes = GrantTypes.Code,
                AllowOfflineAccess = true,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "enterprise",
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:4200",
                },
                RedirectUris =
                {
                    "http://localhost:4200",
                    "http://localhost:4200/silent-refresh.html",
                },
                PostLogoutRedirectUris =
                {
                    "http://localhost:4200",
                },
            },
        };
}
