﻿using System.ComponentModel.DataAnnotations;

namespace QuizBuilder.API.DTOs.Account
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,}$", ErrorMessage = "Password Must be Complex")]
        public string Password { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
