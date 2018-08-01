﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourcingTool.Models
{
    public class UserErrors
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string ErrorMessage { get; set; }
    }
}