﻿namespace Domain.Identity
{
    public class JwtToken
    {
        public string UserId { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
