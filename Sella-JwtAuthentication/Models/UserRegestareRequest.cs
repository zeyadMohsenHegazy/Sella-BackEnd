﻿using System.ComponentModel.DataAnnotations;

namespace API_Sella.Models
{
    public class UserRegestareRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
