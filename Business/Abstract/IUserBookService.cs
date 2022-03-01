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
       IDataResult<List<Author>> GetAuthorsFromUsersLibrary(int userId); //fe de kullanıcının kütüphanesindeki yazarlara ulaşmak için
       IDataResult<List<Genre>> GetGenresFromUsersLibrary(int userId); //fe de kullanıcının kütüphanesindeki türlere ulaşmak için
       IDataResult<List<Publisher>> GetPublishersFromUsersLibrary(int userId); //fe de kullanıcının kütüphanesindeki yayınevlerine ulaşmak için
       IDataResult<List<BookForUserDto>> GetByNoteIncluded(int userId);
       IDataResult<List<BookForUserDto>> GetByPublisherId(int userId,int publisherId); //pipe
       IDataResult<List<BookForUserDto>> GetByAuthorId(int userId, int authorId); //pipe
       IDataResult<List<BookForUserDto>> GetByAuthorNativeStatue(int userId, bool native); //pipe
       IDataResult<List<BookForUserDto>> GetByGenreId(int userId, int genreId); //pipe
       IDataResult<List<BookForUserDto>> GetByReadStatue(int userId, bool readStatue); //pipe
       IDataResult<List<UserBook>> GetAllUserBooks(int userId); //user manager da kullanılacak. API de olmasına gerek yok.
       IResult Add(UserBook userBook);
       IResult Update(UserBook userBook);
       IResult Delete(UserBook userBook);

   }
}
