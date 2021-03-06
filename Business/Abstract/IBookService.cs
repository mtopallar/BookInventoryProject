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
    public interface IBookService
    {
        IDataResult<List<Book>> GetAll(); // GetAllForAddToLibrary(); yeterli olabilir. apide yok
        IDataResult<Book> GetById(int id); // apiden sildim GetByIdForAddToLibrary(int id) yi ekledim yerine yetkiyi user a indirdim apide yok zaten userbookmanager kullanıyor.
        IDataResult<List<BookForAddToLibraryDto>> GetAllForAddToLibrary();
        IDataResult<BookForAddToLibraryDto> GetByIdForAddToLibrary(int id);
        IDataResult<BookForAddToLibraryDto> GetByIsbnForAddToLibrary(string isbn); //pipe ile çözülebilir
        IDataResult<List<BookForAddToLibraryDto>> GetListByBookNameForAddToLibrary(string bookName); //pipe
        IDataResult<List<BookForAddToLibraryDto>> GetListByPublisherIdForAddToLibrary(int publisherId); //pipe
        IDataResult<List<BookForAddToLibraryDto>> GetListByAuthorIdForAddToLibrary(int authorId); //pipe
        IDataResult<List<BookForAddToLibraryDto>> GetListByNativeStatueForAddToLibrary(bool native); //pipe
        IDataResult<List<BookForAddToLibraryDto>> GetListByGenreIdForAddToLibrary(int genreId); //pipe
        IResult Add(Book book);
        IResult Update(Book book); //id ye göre

    }
}
