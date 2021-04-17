using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
   public interface IUserBookService
   {
       IDataResult<List<BookForUserDto>> GetAll(int userId);
       IDataResult<List<BookForUserDto>> GetByNoteIncluded(int userId);
       IDataResult<List<BookForUserDto>> GetByPublisherId(int userId,int publisherId);
       IDataResult<List<BookForUserDto>> GetByAuthorId(int userId, int authorId);
       IDataResult<List<BookForUserDto>> GetByGenreId(int userId, int genreId);
       IDataResult<List<BookForUserDto>> GetByReadStatue(int userId, bool readStatue);
       IResult Add(int userId, int bookId);
       IResult Update(int userId, int bookId);
       IResult Delete(int userId, int bookId);

   }
}
