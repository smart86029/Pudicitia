using System;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pudicitia.Identity.Api.Models.Authentication;
using Pudicitia.Identity.App.Authentication;

namespace Pudicitia.Identity.Api.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IEventService eventService;
        private readonly AuthenticationApp accountApp;

        public AuthenticationController(
            IIdentityServerInteractionService interactionService,
            IEventService eventService,
            AuthenticationApp accountApp)
        {
            this.interactionService = interactionService;
            this.eventService = eventService;
            this.accountApp = accountApp;
        }

        [HttpGet]
        public async Task<IActionResult> SignInAsync([FromQuery] string returnUrl)
        {
            var context = await interactionService.GetAuthorizationContextAsync(returnUrl);
            var model = new SignInViewModel
            {
                UserName = context?.LoginHint,
                ReturnUrl = returnUrl,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignInAsync([FromForm] SignInViewModel model)
        {
            var context = await interactionService.GetAuthorizationContextAsync(model.ReturnUrl);
            if (!ModelState.IsValid)
                return View(model);

            var user = await accountApp.GetUserAsync(model.UserName, model.Password);
            if (user == default)
            {
                await eventService.RaiseAsync(new UserLoginFailureEvent(model.UserName, "invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(model);
            }

            var subjectId = user.Id.ToString();
            await eventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, subjectId, user.UserName, clientId: context?.Client.ClientId));

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
                return Redirect(model.ReturnUrl);
            if (Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);
            if (string.IsNullOrEmpty(model.ReturnUrl))
                return Redirect("~/");

            throw new Exception("invalid return URL");
        }

        [HttpGet]
        public async Task<IActionResult> SignOutAsync([FromQuery] string signOutId)
        {
            var model = new SignOutViewModel
            {
                SignOutId = signOutId,
            };

            if (!User.Identity.IsAuthenticated)
                return await SignOutAsync(model);

            var context = await interactionService.GetLogoutContextAsync(signOutId);
            if (!context.ShowSignoutPrompt)
                return await SignOutAsync(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOutAsync([FromForm] SignOutViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                await eventService.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            var context = await interactionService.GetLogoutContextAsync(model.SignOutId);

            return Redirect(context?.PostLogoutRedirectUri);
        }
    }
}