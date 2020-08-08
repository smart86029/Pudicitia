using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pudicitia.Identity.Api.Models.Account;
using Pudicitia.Identity.App.Account;

namespace Pudicitia.Identity.Api.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountApp accountApp;

        public AccountController(AccountApp accountApp)
        {
            this.accountApp = accountApp;
        }

        [HttpGet]
        public IActionResult Authorize([FromQuery] AuthorizeViewModel model)
        {
            var command = new CreateAuthorizationCodeCommand
            {
                ResponseType = model.ResponseType,
                ClientId = model.ClientId,
                RedirectUri = model.RedirectUri,
                Scope = model.Scope,
                State = model.State,
            };

            return View(command);
        }

        [HttpPost("Account/SignIn")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignInAsync([FromForm] CreateAuthorizationCodeCommand command)
        {
            var result = await accountApp.CreateAuthorizationCodeAsync(command);

            return Redirect(result.RedirectUri);
        }

        //[HttpPost("token")]
        //public async Task<IActionResult> TokenAsync(CreateTokenCommand command)
        //{
        //    var result = await accountApp.CreateTokenAsync(command);

        //    return View(result);
        //}
    }
}
