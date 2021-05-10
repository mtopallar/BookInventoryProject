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
        IDataResult<List<Book>> GetAll();
        IDataResult<Book> GetById(int id);
        IDataResult<List<BookForAddToLibraryDto>> GetAllForAddToLibrary();
        IDataResult<BookForAddToLibraryDto> GetByIsbnForAddToLibrary(string isbn); //pipe ile çözülebilir
        IDataResult<List<BookForAddToLibraryDto>> GetLisyByBookNameForAddToLibrary(string bookName); //pipe
        IDataResult<List<BookForAddToLibraryDto>> GetListByPublisherIdForAddToLibrary(int publisherId); //pipe
        IDataResult<List<BookForAddToLibraryDto>> GetListByAuthorIdForAddToLibrary(int authorId); //pipe
        IDataResult<List<BookForAddToLibraryDto>> GetListByCountryIdForAddToLibrary(int nationalityId); //pipe
        IDataResult<List<BookForAddToLibraryDto>> GetListByGenreIdForAddToLibrary(int genreId); //pipe
        IResult Add(Book book);
        IResult Update(Book book);
        
    } 
}
