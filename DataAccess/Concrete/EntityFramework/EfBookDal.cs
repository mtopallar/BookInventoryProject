using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.Concrete.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfBookDal:EfEntityRepositoryBase<Book,BookInventoryProjectContext>,IBookDal
    {
        public List<BookForAddToLibraryDto> GetBooksForAddToLibrary(Expression<Func<BookForAddToLibraryDto, bool>> filter = null)
        {
            using (var context = new BookInventoryProjectContext())
            {
                var result = from book in context.Books
                    join publisher in context.Publishers on book.PublisherId equals publisher.Id
                    join author in context.Authors on book.AuthorId equals author.Id
                    join genre in context.Genres on book.GenreId equals genre.Id
                    select new BookForAddToLibraryDto
                    {
                        BookId = book.Id,
                        Name = book.Name,
                        Isbn = book.Isbn,
                        PublisherName = publisher.Name,
                        AuthorFullName = $"{author.FirstName} {author.LastName}",
                        Native = author.Native,
                        GenreName = genre.Name
                    };

                return filter == null ? result.ToList() : result.Where(filter).ToList(); 
            }
        }
    }
}
