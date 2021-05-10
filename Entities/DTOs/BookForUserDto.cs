using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Abstract;

namespace Entities.DTOs
{
    public class BookForUserDto:BookForAddToLibraryDto, IDto
    {
        public int UserId { get; set; }
        public bool ReadStatue { get; set; }
        public string NoteDetail { get; set; }
    }
}
