using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
    public class UserBookManager : IUserBookService
    {
        private readonly IUserBookDal _userBookDal;
        private readonly IPublisherService _publisherService;
        private readonly IAuthorService _authorService;
        private readonly IGenreService _genreService;


        public UserBookManager(IUserBookDal userBookDal, IPublisherService publisherService, IAuthorService authorService, IGenreService genreService)
        {
            _userBookDal = userBookDal;
            _publisherService = publisherService;
            _authorService = authorService;
            _genreService = genreService;
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetAll(int userId)
        {
            var result = GetBookNotes(_userBookDal.GetBookWithDetails(b => b.UserId == userId));
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUsersAllBooksSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByNoteIncluded(int userId)
        {
            var result = GetBookNotes(_userBookDal.GetBookWithDetails(b => b.UserId == userId && b.NoteDetail != null));
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUsersAllBooksWichHasNoteSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByPublisherId(int userId, int publisherId)
        {
            var getPublisher = _publisherService.GetById(publisherId).Data;
            var result =
                GetBookNotes(_userBookDal.GetBookWithDetails(b =>
                    b.UserId == userId && b.PublisherName == getPublisher.Name));
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUsersAllBooksByPublisherId);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByAuthorId(int userId, int authorId)
        {
            var getAuthor = _authorService.GetById(authorId).Data;
            var result = GetBookNotes(_userBookDal.GetBookWithDetails(b =>
                b.UserId == userId && b.AuthorFullName == $"{getAuthor.FirstName} {getAuthor.LastName}"));
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUsersAllBooksByAuthorId);
        }

        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByAuthorNativeStatue(int userId, bool native)
        {
            var result = GetBookNotes(_userBookDal.GetBookWithDetails(b => b.Native == native));
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUserBooksByNativeStatueSuccessfully);
        }

        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByGenreId(int userId, int genreId)
        {
            var getGenre = _genreService.GetById(genreId).Data;
            var result =
                GetBookNotes(_userBookDal.GetBookWithDetails(b => b.UserId == userId && b.GenreName == getGenre.Name));
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUsersAllBookByGenreIdSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByReadStatue(int userId, bool readStatue)
        {
            var result =
                GetBookNotes(_userBookDal.GetBookWithDetails(b => b.UserId == userId && b.ReadStatue == readStatue));
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUsersAllBookByReadStatueSuccessfully);
        }
        [SecuredOperation("user,admin")]
        public IDataResult<List<UserBook>> GetAllUserBooks(int userId)
        {
            return new SuccessDataResult<List<UserBook>>(_userBookDal.GetAll(u => u.UserId == userId),
                Messages.GetAllUserBookEntitiesSuccessfully);
        }

        [SecuredOperation("user")]
        public IResult Add(UserBook userBook)
        {
            var result = BusinessRules.Run(CheckThisBookAddedUserLibraryBefore(userBook));

            if (result != null)
            {
                return result;
            }
            _userBookDal.Add(userBook);
            return new SuccessResult(Messages.UserBookAddedSuccessfully);
        }
        [SecuredOperation("user")]
        public IResult Update(UserBook userBook)
        {
            var result = BusinessRules.Run(CheckThisBookAddedUserLibraryBefore(userBook));

            if (result != null)
            {
                return result;
            }
            _userBookDal.Update(userBook);
            return new SuccessResult(Messages.UserBookUpdatedSuccessfully);
        }
        [SecuredOperation("admin,user")]
        public IResult Delete(UserBook userBook)
        {
            _userBookDal.Delete(userBook);
            return new SuccessResult(Messages.UserBookDeletedSuccessfully);
        }

        private List<BookForUserDto> GetBookNotes(List<BookForUserDto> bookForUserDtos)
        {
            foreach (BookForUserDto bookForUserDto in bookForUserDtos)
            {
                var getNotes = _userBookDal.GetAll(u =>
                    u.UserId == bookForUserDto.UserId && u.BookId == bookForUserDto.BookId);
                foreach (UserBook userBook in getNotes)
                {
                    bookForUserDto.NoteDetail = userBook.Note;
                }
            }

            return bookForUserDtos;
        }

        private IResult CheckThisBookAddedUserLibraryBefore(UserBook userBook)
        {
            var tryGetUserBook = _userBookDal.Get(u => u.UserId == userBook.UserId && u.BookId == userBook.BookId);

            if (tryGetUserBook != null)
            {
                return new ErrorResult(Messages.UserBookAddedAlready);
            }

            return new SuccessResult();
        }

    }
}
