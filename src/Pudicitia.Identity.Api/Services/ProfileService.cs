using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Pudicitia.Common.Extensions;
using Pudicitia.Common.Identity;
using Pudicitia.Identity.App.Authentication;

namespace Pudicitia.Identity.Api.Services;

public class ProfileService : IProfileService
{
    private readonly AuthenticationApp _authentication;

    public ProfileService(AuthenticationApp authentication)
    {
        _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var userId = context.Subject.FindFirst(JwtClaimTypes.Subject)!.Value.ToGuid();
        var permissionCodes = await _authentication.GetPermissionCodesAsync(userId);
        var claims = permissionCodes.Select(x => new Claim(IdentityClaimTypes.Permission, x));
        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        await Task.CompletedTask;
    }
}
