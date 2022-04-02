using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Abstract;

namespace Entities.DTOs
{
    public class DeleteForAdminWithAttemptingUserIdDto:IDto
    {
        public int UserId { get; set; }
        public int AttemptingUserId { get; set; }
    }
}
