﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Telefon_Rehberi.Models.Account
{
    public class ChangePassword
    {
        public Database.Logins admin { get; set; }
        public string password { get; set; }
        public string currentPassword { get; set; }
    }
}