using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataBaseModels.Identity
{
    public class UserLoginInfo
    {
        /// <summary>
        /// Gets or sets the provider for this instance of Microsoft.AspNetCore.Identity.UserLoginInfo.
        /// Examples of the provider may be Local, Facebook, Google, etc.
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user identity user provided by the
        /// login provider. This would be unique per provider, examples may be @microsoft as a 
        /// Twitter provider key.
        /// </summary>
        public string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the display name for the provider.
        /// </summary>
        public string ProviderDisplayName { get; set; }
    }
}
