﻿using System.ComponentModel.DataAnnotations;

namespace Pudicitia.Identity.Api.Models.Account
{
    public class SignInViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}