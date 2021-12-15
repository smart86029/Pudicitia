using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pudicitia.Identity.Api.Pages.Authentication;

public class SignOutModel : PageModel
{
    private readonly IIdentityServerInteractionService _interactionService;
    private readonly IEventService _eventService;

    public SignOutModel(
        IIdentityServerInteractionService interactionService,
        IEventService eventService)
    {
        _interactionService = interactionService ?? throw new ArgumentNullException(nameof(interactionService));
        _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
    }

    public string SignOutId { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync([FromQuery] string signOutId)
    {
        SignOutId = signOutId;

        if (!User.Identity!.IsAuthenticated)
        {
            return await OnPostAsync();
        }

        var context = await _interactionService.GetLogoutContextAsync(signOutId);
        if (!context.ShowSignoutPrompt)
        {
            return await OnPostAsync();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (User.Identity!.IsAuthenticated)
        {
            await HttpContext.SignOutAsync();
            await _eventService.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
        }

        var context = await _interactionService.GetLogoutContextAsync(SignOutId);

        return Redirect(context.PostLogoutRedirectUri);
    }
}
