﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Sella_DashBoard.Areas.Identity.Data;

// Add profile data for application users by adding properties to the Sella_DashBoardUser class
public class Sella_DashBoardUser : IdentityUser
{
    public string FirstName { get; set; }
}

