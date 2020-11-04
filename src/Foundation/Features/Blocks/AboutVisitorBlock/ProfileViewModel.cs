﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundation.Features.Blocks.AboutVisitorBlock
{
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {

        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string GetAddress()
        {
            if (string.IsNullOrEmpty(City) || string.IsNullOrEmpty(State)) return "N/A";
            return string.Join(", ", City, State);
        }
    }
}