using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Pudicitia.Identity.App.Authorization;

namespace Pudicitia.Identity.Api
{
    public class AuthorizationService : Authorization.AuthorizationBase
    {
        private readonly AuthorizationApp authorizationApp;

        public AuthorizationService(AuthorizationApp authorizationApp)
        {
            this.authorizationApp = authorizationApp;
        }

        public override async Task<ListRolesResponse> ListRoles(ListRolesRequest request, ServerCallContext context)
        {
            var options = new RoleOptions
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            var roles = await authorizationApp.GetRolesAsync(options);
            var items = roles.Items.Select(x => new Role
            {
                Id = x.Id,
                Name = x.Name,
                IsEnabled = x.IsEnabled,
            });
            var result = new ListRolesResponse
            {
                PageIndex = options.PageIndex,
                PageSize = options.PageSize,
                ItemCount = roles.ItemCount,
            };
            result.Items.AddRange(items);

            return result;
        }
    }
}