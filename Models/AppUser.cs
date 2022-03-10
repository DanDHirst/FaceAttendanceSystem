using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Models
{
    public class AppUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserRole { get; set; }

        public string GetRole()
        {
            return this.UserRole;
        }
        public string GetFirstName()
        {
            return this.Firstname;
        }
        public string GetLastname()
        {
            return this.Lastname;
        }
    }
}
