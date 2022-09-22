using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Pudicitia.Common.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        var nameIdentifier = context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var result = nameIdentifier?.ToGuid() ?? Guid.Empty;

        return result;
    }
}
