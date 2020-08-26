using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Pudicitia.Common.Extensions;
using Pudicitia.Common.Utilities;
using Pudicitia.Identity.Domain;
using Pudicitia.Identity.Domain.Permissions;
using Pudicitia.Identity.Domain.Roles;
using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.App.Account
{
    public class AccountApp
    {
        private readonly IIdentityUnitOfWork unitOfWork;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly JwtSettings jwtSettings;

        public AccountApp(
            IIdentityUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository,
            JwtSettings jwtSettings)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            this.permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
            this.jwtSettings = jwtSettings;
        }

        public async Task<UserDetail> GetUserAsync(string userName, string password)
        {
            var user = await userRepository.GetUserAsync(userName, password);
            var result = new UserDetail
            {
                Id = user.Id,
                UserName = user.UserName,
            };

            return result;
        }

        public async Task<TokenDetail> RefreshTokenAsync(RefreshTokenCommand command)
        {
            var principal = jwtSettings.GetPrincipal(command.AccessToken);
            Guid.TryParse(principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value, out var userId);
            var user = await userRepository.GetUserAsync(userId);
            if (user == default)
                return default;

            if (!user.IsValidRefreshToken(command.RefreshToken))
                return default;

            var accessToken = jwtSettings.CreateAccessToken(user);
            var refreshToken = jwtSettings.CreateRefreshToken();
            user.RemoveRefreshToken(command.RefreshToken);
            user.AddRefreshToken(refreshToken, jwtSettings.RefreshTokenExpiry);

            userRepository.Update(user);
            await unitOfWork.CommitAsync();

            var token = new TokenDetail
            {
                AccessToken = accessToken,
                TokenType = "Bearer",
                ExpiresIn = jwtSettings.AccessTokenExpiry.TotalSeconds.ToInt(),
                RefreshToken = refreshToken
            };

            return token;
        }

        private async Task<List<string>> GetPermissionCodesAsync(User user)
        {
            var roleIds = user.UserRoles.Select(x => x.RoleId);
            var roles = await roleRepository.GetRolesAsync(r => roleIds.Contains(r.Id));
            var permissionIds = roles.SelectMany(r => r.RolePermissions).Select(x => x.PermissionId).Distinct();
            var permissions = await permissionRepository.GetPermissionsAsync(p => permissionIds.Contains(p.Id));
            var result = permissions.Select(p => p.Code).ToList();

            return result;
        }
    }
}