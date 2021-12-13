using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pudicitia.Identity.Api.Pages.Authentication
{
    public class SignOutModel : PageModel
    {
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IEventService eventService;

        public SignOutModel(
            IIdentityServerInteractionService interactionService,
            IEventService eventService)
        {
            this.interactionService = interactionService;
            this.eventService = eventService;
        }

        public string SignOutId { get; set; }

        public async Task<IActionResult> OnGetAsync([FromQuery] string signOutId)
        {
            SignOutId = signOutId;

            if (!User.Identity.IsAuthenticated)
                return await OnPostAsync();

            var context = await interactionService.GetLogoutContextAsync(signOutId);
            if (!context.ShowSignoutPrompt)
                return await OnPostAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                await eventService.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            var context = await interactionService.GetLogoutContextAsync(SignOutId);

            return Redirect(context?.PostLogoutRedirectUri);
        }
    }
}