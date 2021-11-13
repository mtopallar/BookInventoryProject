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
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.StringEditor;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
    public class BookManager : IBookService
    {
        private readonly IBookDal _bookDal;
        private readonly IPublisherService _publisherService;
        private readonly IAuthorService _authorService;
        private readonly IGenreService _genreService;

        public BookManager(IBookDal bookDal, IPublisherService publisherService, IAuthorService authorService, IGenreService genreService)
        {
            _bookDal = bookDal;
            _publisherService = publisherService;
            _authorService = authorService;
            _genreService = genreService;
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
        public IDataResult<List<BookForAddToLibraryDto>> GetByIdForAddToLibrary(int id)
        {
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(
                _bookDal.GetBooksForAddToLibrary(b => b.BookId == id), Messages.GetBookByIdForAddToLibrarySuccessfully);
        }

        [SecuredOperation("admin,book.admin,user")]
        public IDataResult<BookForAddToLibraryDto> GetByIsbnForAddToLibrary(string isbn)
        {
            return new SuccessDataResult<BookForAddToLibraryDto>(_bookDal.GetBooksForAddToLibrary(b => b.Isbn == isbn).FirstOrDefault(),
                Messages.GetBookForAddToLibraryByIsbnSuccessfully);
        }
        [SecuredOperation("admin,book.admin,user")]
        public IDataResult<List<BookForAddToLibraryDto>> GetListByBookNameForAddToLibrary(string bookName)
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
        public IDataResult<List<BookForAddToLibraryDto>> GetListByNativeStatueForAddToLibrary(bool native)
        {
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(
                _bookDal.GetBooksForAddToLibrary(b => b.Native == native),
                Messages.GetBooksForAddToLibraryListByNativeStatueSuccessfully);
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
            var isBookAddedAlreadyBefore = BusinessRules.Run(IsBookAddedAlreadyBefore(book));

            if (isBookAddedAlreadyBefore != null)
            {
                return isBookAddedAlreadyBefore;
            }

            book.Name = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(book.Name));
            _bookDal.Add(book);
            return new SuccessResult(Messages.BookAddedSuccessfully);
        }
        [SecuredOperation("admin,book.admin")]
        [CacheRemoveAspect("IBookService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(BookValidator))]
        public IResult Update(Book book)
        {
            var tryToGetBook = _bookDal.Get(b => b.Id == book.Id);
            tryToGetBook.Name = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(book.Name));
            tryToGetBook.Isbn = book.Isbn;
            tryToGetBook.PublisherId = book.PublisherId;
            tryToGetBook.AuthorId = book.AuthorId;
            tryToGetBook.GenreId = book.GenreId;
            _bookDal.Update(tryToGetBook);
            return new SuccessResult(Messages.BookUpdatedSuccessfully);
        }

        private IResult IsBookAddedAlreadyBefore(Book book)
        {
            var bookNameTryToFind =
                StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(book.Name));
            var tryGetBook = _bookDal.Get(b =>
                b.Name == bookNameTryToFind && b.Isbn == book.Isbn && b.PublisherId == book.PublisherId);
            if (tryGetBook != null)
            {
                return new ErrorResult(Messages.BookAddedAlreadyBefore);
            }

            return new SuccessResult();
        }


    }
}
