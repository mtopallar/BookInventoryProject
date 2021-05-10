using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
   public class BookManager:IBookService
   {
       private readonly IBookDal _bookDal;
       private readonly IPublisherService _publisherService;
       private readonly IAuthorService _authorService;
       private readonly IGenreService _genreService;
       private readonly INationalityService _nationalityService;

       public BookManager(IBookDal bookDal, IPublisherService publisherService, IAuthorService authorService, IGenreService genreService, INationalityService nationalityService)
       {
           _bookDal = bookDal;
           _publisherService = publisherService;
           _authorService = authorService;
           _genreService = genreService;
           _nationalityService = nationalityService;
       }
        [SecuredOperation("admin,book.admin")]
        [CacheAspect()]
       public IDataResult<List<Book>> GetAll()
       {
           return new SuccessDataResult<List<Book>>(_bookDal.GetAll(), Messages.GetAllBooksSuccessfully);
       }
        [SecuredOperation("admin,book.admin")]
        public IDataResult<Book> GetById(int id)
        {
            return new SuccessDataResult<Book>(_bookDal.Get(b => b.Id == id), Messages.GetBookByIdSuccessfully);
        }
        [SecuredOperation("admin,book.admin,user")]
        [CacheAspect()]
        public IDataResult<List<BookForAddToLibraryDto>> GetAllForAddToLibrary()
        {
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(_bookDal.GetBooksForAddToLibrary(),
                Messages.GetAllBooksForAddToLibrarySuccessfully);
        }
        [SecuredOperation("admin,book.admin,user")]
        public IDataResult<BookForAddToLibraryDto> GetByIsbnForAddToLibrary(string isbn)
        {
            return new SuccessDataResult<BookForAddToLibraryDto>(_bookDal.GetBooksForAddToLibrary(b => b.Isbn == isbn).FirstOrDefault(),
                Messages.GetBookForAddToLibraryByIsbnSuccessfully);
        }
        [SecuredOperation("admin,book.admin,user")]
        public IDataResult<List<BookForAddToLibraryDto>> GetLisyByBookNameForAddToLibrary(string bookName)
        {
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(
                _bookDal.GetBooksForAddToLibrary(b => b.Name == bookName),
                Messages.GetBookForAddToLibraryByBookNameSuccessfully);
        }
        [SecuredOperation("admin,book.admin,user")]
        public IDataResult<List<BookForAddToLibraryDto>> GetListByPublisherIdForAddToLibrary(int publisherId)
        {
            var publisher = _publisherService.GetById(publisherId).Data;
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(
                _bookDal.GetBooksForAddToLibrary(b => b.PublisherName == publisher.Name),
                Messages.GetBookForAddToLibraryByPublisherIdSuccessfully);
        }
        [SecuredOperation("admin,book.admin,user")]
        public IDataResult<List<BookForAddToLibraryDto>> GetListByAuthorIdForAddToLibrary(int authorId)
        {
            var author = _authorService.GetById(authorId).Data;
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(
                _bookDal.GetBooksForAddToLibrary(b => b.AuthorFullName == $"{author.FirstName} {author.LastName}"),
                Messages.GetBookForAddToLibraryByAuthorIdSuccessfully);
        }
        [SecuredOperation("admin,book.admin,user")]
        public IDataResult<List<BookForAddToLibraryDto>> GetListByCountryIdForAddToLibrary(int nationalityId)
        {
            var nationality = _nationalityService.GetById(nationalityId).Data;
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(
                _bookDal.GetBooksForAddToLibrary(b => b.CountryName == nationality.CountryName),
                Messages.GetBookForAddToLibraryByNationalityIdSuccessfully);
        }
        [SecuredOperation("admin,book.admin,user")]
        public IDataResult<List<BookForAddToLibraryDto>> GetListByGenreIdForAddToLibrary(int genreId)
        {
            var genre = _genreService.GetById(genreId).Data;
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(
                _bookDal.GetBooksForAddToLibrary(b => b.GenreName == genre.Name),
                Messages.GetBookForAddToLibraryByGenreIdSuccessfully);
        }

        [SecuredOperation("admin,book.admin")]
        [CacheRemoveAspect("IBookService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(BookValidator))]
        public IResult Add(Book book)
        {
            _bookDal.Add(book);
            return new SuccessResult(Messages.BookAddedSuccessfully);
        }
        [SecuredOperation("admin,book.admin")]
        [CacheRemoveAspect("IBookService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(BookValidator))]
        public IResult Update(Book book)
        {
            _bookDal.Update(book);
            return new SuccessResult(Messages.BookUpdatedSuccessfully);
        }
        
    }
}
