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
    public class EfUserBookDal:EfEntityRepositoryBase<UserBook,BookInventoryProjectContext>,IUserBookDal
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
                    join nationality in context.Nationalities on author.NationalityId equals nationality.Id
                    select new BookForUserDto
                    {
                        UserId = userbook.UserId,
                        BookId = book.Id,
                        Name = book.Name,
                        Isbn = book.Isbn,
                        PublisherName = publisher.Name,
                        AuthorName = author.FirstName,
                        AuthorLastName = author.LastName,
                        CountryName = nationality.CountryName,
                        GenreName = genre.Name,
                        ReadStatue = userbook.ReadStatue
                    };

                return result.Where(filter).ToList();
            }
        }
    }
}
