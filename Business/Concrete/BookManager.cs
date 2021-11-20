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
            var result = _bookDal.GetAll();
            if (result.Count==0)
            {
                return new ErrorDataResult<List<Book>>(Messages.CanNotFindAnyBook);
            }
            return new SuccessDataResult<List<Book>>(result, Messages.GetAllBooksSuccessfully);
        }
        [SecuredOperation("admin,book.admin")]
        public IDataResult<Book> GetById(int id)
        {
            var result = _bookDal.Get(b => b.Id == id);
            if (result==null)
            {
                return new ErrorDataResult<Book>(Messages.WrongBookId);
            }
            return new SuccessDataResult<Book>(result, Messages.GetBookByIdSuccessfully);
        }
        [SecuredOperation("user")]
        [CacheAspect()]
        public IDataResult<List<BookForAddToLibraryDto>> GetAllForAddToLibrary()
        {
            var result = _bookDal.GetBooksForAddToLibrary();
            if (result.Count==0)
            {
                return new ErrorDataResult<List<BookForAddToLibraryDto>>(Messages.CanNotFindAnyBook);
            }
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(result, Messages.GetAllBooksForAddToLibrarySuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<BookForAddToLibraryDto> GetByIdForAddToLibrary(int id)
        {
            var result = _bookDal.GetBooksForAddToLibrary(b => b.BookId == id);
            if (result.Count==0)
            {
                return new ErrorDataResult<BookForAddToLibraryDto>(Messages.WrongBookId);
            }
            return new SuccessDataResult<BookForAddToLibraryDto>(result.Single(), Messages.GetBookByIdForAddToLibrarySuccessfully);
        }

        [SecuredOperation("user")]
        public IDataResult<BookForAddToLibraryDto> GetByIsbnForAddToLibrary(string isbn)
        {
            var result = _bookDal.GetBooksForAddToLibrary(b => b.Isbn == isbn);
            if (result.Count==0)
            {
                return new ErrorDataResult<BookForAddToLibraryDto>(Messages.WrongIsbnNumber);
            }
            return new SuccessDataResult<BookForAddToLibraryDto>(result.Single(), Messages.GetBookForAddToLibraryByIsbnSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForAddToLibraryDto>> GetListByBookNameForAddToLibrary(string bookName)
        {
            var result = _bookDal.GetBooksForAddToLibrary(b => b.Name == bookName);
            if (result.Count==0)
            {
                return new ErrorDataResult<List<BookForAddToLibraryDto>>(Messages.WrongBookName);
            }
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(result, Messages.GetBookForAddToLibraryByBookNameSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForAddToLibraryDto>> GetListByPublisherIdForAddToLibrary(int publisherId)
        {
            var publisher = _publisherService.GetById(publisherId);
            if (!publisher.Success)
            {
                return new ErrorDataResult<List<BookForAddToLibraryDto>>(publisher.Message);
            }
            var result = _bookDal.GetBooksForAddToLibrary(b => b.PublisherName == publisher.Data.Name);
            if (result.Count==0)
            {
                return new ErrorDataResult<List<BookForAddToLibraryDto>>(Messages.WrongPublisher);
            }
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(result, Messages.GetBookForAddToLibraryByPublisherIdSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForAddToLibraryDto>> GetListByAuthorIdForAddToLibrary(int authorId)
        {
           var author = _authorService.GetById(authorId);
           if (!author.Success)
           {
               return new ErrorDataResult<List<BookForAddToLibraryDto>>(author.Message);
           }

           var result = _bookDal.GetBooksForAddToLibrary(b => b.AuthorFullName == $"{author.Data.FirstName} {author.Data.LastName}");

           if (result.Count==0)
           {
               return new ErrorDataResult<List<BookForAddToLibraryDto>>(Messages.WrongAuthor);
           }

           return new SuccessDataResult<List<BookForAddToLibraryDto>>(result, Messages.GetBookForAddToLibraryByAuthorIdSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForAddToLibraryDto>> GetListByNativeStatueForAddToLibrary(bool native)
        {
            var result = _bookDal.GetBooksForAddToLibrary(b => b.Native == native);
            if (result.Count==0)
            {
                return new ErrorDataResult<List<BookForAddToLibraryDto>>(Messages.NoBookByThisNativeSelection);
            }
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(result, Messages.GetBooksForAddToLibraryListByNativeStatueSuccessfully);
        }


        [SecuredOperation("user")]
        public IDataResult<List<BookForAddToLibraryDto>> GetListByGenreIdForAddToLibrary(int genreId)
        {
            var genre = _genreService.GetById(genreId);

            if (!genre.Success)
            {
                return new ErrorDataResult<List<BookForAddToLibraryDto>>(genre.Message);
            }

            var result = _bookDal.GetBooksForAddToLibrary(b => b.GenreName == genre.Data.Name);

            if (result.Count==0)
            {
                return new ErrorDataResult<List<BookForAddToLibraryDto>>(Messages.WrongGenre);
            }
            return new SuccessDataResult<List<BookForAddToLibraryDto>>(result, Messages.GetBookForAddToLibraryByGenreIdSuccessfully);
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
            var checkNewBookBeforeUpdateIsBookAddedBefore = BusinessRules.Run(IsBookAddedAlreadyBefore(book));
            if (checkNewBookBeforeUpdateIsBookAddedBefore!=null)
            {
                return checkNewBookBeforeUpdateIsBookAddedBefore;
            }
            var tryToGetBook = GetById(book.Id);
            if (!tryToGetBook.Success)
            {
                return new ErrorResult(tryToGetBook.Message);
            }
            tryToGetBook.Data.Name = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(book.Name));
            tryToGetBook.Data.Isbn = book.Isbn;
            tryToGetBook.Data.PublisherId = book.PublisherId;
            tryToGetBook.Data.AuthorId = book.AuthorId;
            tryToGetBook.Data.GenreId = book.GenreId;
            _bookDal.Update(tryToGetBook.Data);
            return new SuccessResult(Messages.BookUpdatedSuccessfully);
        }

        private IResult IsBookAddedAlreadyBefore(Book book)
        {
            // Bir kitabın aynı isbn numarasını kullnabilmesi için şartları araştırıp metodu ona göre yazdım. (Name, ISNB, Publisher bağlı)

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
