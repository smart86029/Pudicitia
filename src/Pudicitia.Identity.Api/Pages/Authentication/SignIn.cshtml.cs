using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pudicitia.Identity.App.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Pudicitia.Identity.Api.Pages.Authentication
{
    public class SignInModel : PageModel
    {
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IEventService _eventService;
        private readonly AuthenticationApp _accountApp;

        public SignInModel(
            IIdentityServerInteractionService interactionService,
            IEventService eventService,
            AuthenticationApp accountApp)
        {
            _interactionService = interactionService;
            _eventService = eventService;
            _accountApp = accountApp;
        }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            var context = await _interactionService.GetAuthorizationContextAsync(returnUrl);

            UserName = context?.LoginHint ?? string.Empty;
            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var context = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);
            if (!ModelState.IsValid)
                return Page();

            var user = await _accountApp.GetUserAsync(UserName, Password);
            if (user is null)
            {
                await _eventService.RaiseAsync(new UserLoginFailureEvent(UserName, "invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return Page();
            }

            var subjectId = user.Id.ToString();
            await _eventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, subjectId, user.UserName, clientId: context?.Client.ClientId));

            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(30)),
            };

            var identityServerUser = new IdentityServerUser(subjectId)
            {
                DisplayName = user.UserName
            };
            await HttpContext.SignInAsync(identityServerUser, properties);

            if (context != default)
                return Redirect(ReturnUrl);
            if (Url.IsLocalUrl(ReturnUrl))
                return Redirect(ReturnUrl);
            if (string.IsNullOrEmpty(ReturnUrl))
                return Redirect("~/");

            throw new Exception("invalid return URL");
        }
    }
}