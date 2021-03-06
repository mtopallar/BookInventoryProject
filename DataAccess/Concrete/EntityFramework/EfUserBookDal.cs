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
    public class EfUserBookDal : EfEntityRepositoryBase<UserBook, BookInventoryProjectContext>, IUserBookDal
    {
        public List<BookForUserDto> GetBookWithDetails(Expression<Func<BookForUserDto, bool>> filter)
        {
            using (var context = new BookInventoryProjectContext())
            {
                var result = from userbook in context.UserBooks
                             join book in context.Books on userbook.BookId equals book.Id
                             join publisher in context.Publishers on book.PublisherId equals publisher.Id
                             join author in context.Authors on book.AuthorId equals author.Id
                             join genre in context.Genres on book.GenreId equals genre.Id
                             select new BookForUserDto
                             {   Id = userbook.Id,
                                 UserId = userbook.UserId,
                                 BookId = book.Id,
                                 Name = book.Name,
                                 Isbn = book.Isbn,
                                 PublisherName = publisher.Name,
                                 AuthorFullName = author.FirstName + " " + author.LastName,
                                 Native = author.Native,
                                 GenreName = genre.Name,
                                 ReadStatue = userbook.ReadStatue
                             };

                return result.Where(filter).ToList();
            }
        }
    }
}
