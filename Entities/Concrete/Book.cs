using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class Book:IEntity
    {
        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Name { get; set; }
        public int PublisherId { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
    }
}
