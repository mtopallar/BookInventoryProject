using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Abstract;

namespace Entities.DTOs
{
    public class BookForAddToLibraryDto:IDto
    {
        public int BookId { get; set; }
        public string Isbn { get; set; }
        public string Name { get; set; }
        public string PublisherName { get; set; }
        public string AuthorFullName { get; set; }
        public string CountryName { get; set; }
        public string GenreName { get; set; }
        
    }
}
