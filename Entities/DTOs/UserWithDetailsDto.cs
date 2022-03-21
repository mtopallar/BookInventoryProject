using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Abstract;

namespace Entities.DTOs
{
    public class UserWithDetailsDto:IDto
    {
        public int UserId { get; set; } //sonradan ekledim.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //public string ClaimName { get; set; }
        //public List<string> UserRoleNames { get; set; }
        // eski adı  UserWithDetailsAndRolesDto
    }
}
