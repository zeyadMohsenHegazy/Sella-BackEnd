﻿namespace Sella_JwtAuthentication.Models
{
    public class AuthorRes
    {
        public string Token { get; set; } = string.Empty;
        public bool Result { get; set; } 
        public List<string> Errors { get; set; }
    }
}