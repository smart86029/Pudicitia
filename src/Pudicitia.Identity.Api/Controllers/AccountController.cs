using System;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pudicitia.Identity.Api.Models.Account;
using Pudicitia.Identity.App.Account;

namespace Pudicitia.Identity.Api.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IEventService eventService;
        private readonly AccountApp accountApp;

        public AccountController(
            IAuthenticationSchemeProvider schemeProvider,
            IIdentityServerInteractionService interactionService,
            IEventService eventService,
            AccountApp accountApp)
        {
            this.schemeProvider = schemeProvider;
            this.interactionService = interactionService;
            this.eventService = eventService;
            this.accountApp = accountApp;
        }

        [HttpGet("SignIn")]
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

        [HttpPost("SignIn")]
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
    }
}