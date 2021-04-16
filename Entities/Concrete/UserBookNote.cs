using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class UserBookNote:IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string Note { get; set; }
    }
}
