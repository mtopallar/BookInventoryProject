using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.DTOs
{
    public class UserForUpdateDto:IDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
    }
}
