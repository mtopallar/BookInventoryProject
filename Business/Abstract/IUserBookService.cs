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
       IDataResult<List<BookForUserDto>> GetByPublisherId(int userId,int publisherId); //pipe
       IDataResult<List<BookForUserDto>> GetByAuthorId(int userId, int authorId); //pipe
       IDataResult<List<BookForUserDto>> GetByAuthorNationality(int userId, int nationalityId); //pipe
       IDataResult<List<BookForUserDto>> GetByGenreId(int userId, int genreId); //pipe
       IDataResult<List<BookForUserDto>> GetByReadStatue(int userId, bool readStatue); //pipe
       IDataResult<List<UserBook>> GetAllUserBooks(int userId); //user manager da kullanılacak.
       IResult Add(UserBook userBook);
       IResult Update(UserBook userBook);
       IResult Delete(UserBook userBook);

   }
}
