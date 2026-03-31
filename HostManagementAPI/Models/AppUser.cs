//this is the Identity framework for the user management, it is used to create the user and manage the user authentication and authorization
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HostManagementAPI
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = String.Empty;
    }
}