using System;
using System.ComponentModel.DataAnnotations;
using Domain.Core.Base;
using Domain.DataBaseModels.Identity;

namespace Domain.DataBaseModels.Identity
{
    public class JwtRefreshToken: BaseEntity<long>
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string ClientIp { get; set; }

        [Required]
        public string UserAgent { get; set; }

        [Required]
        public DateTime CreateDateTime { get; set; }
    }
}
