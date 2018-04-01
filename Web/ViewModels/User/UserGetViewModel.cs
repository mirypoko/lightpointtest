﻿using System.Collections.Generic;

namespace Web.ViewModels.User
{
    public class UserGetViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }
    }
}
