using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.StringEditor;
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
            if (result.Count==0)
            {
                return new ErrorDataResult<List<BookForUserDto>>(Messages.ThereAreNoUserBooks);
            }
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUsersAllBooksSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByNoteIncluded(int userId)
        {
            List<BookForUserDto> dtoHasNote = new List<BookForUserDto>();
            var bookDtos = GetAll(userId);
            foreach (var bookForUserDto in bookDtos.Data)
            {
                if (bookForUserDto.NoteDetail != null)
                {
                    dtoHasNote.Add(bookForUserDto);
                }
            }

            if (dtoHasNote.Count == 0)
            {
                return new ErrorDataResult<List<BookForUserDto>>(Messages.NoBookHasNote);
            }

            return new SuccessDataResult<List<BookForUserDto>>(dtoHasNote, Messages.GetUsersAllBooksWichHasNoteSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByPublisherId(int userId, int publisherId)
        {
            var getPublisher = _publisherService.GetById(publisherId).Data;
            var result = GetBookNotes(_userBookDal.GetBookWithDetails(b => b.UserId == userId && b.PublisherName == getPublisher.Name));
            if (result.Count==0)
            {
                return new ErrorDataResult<List<BookForUserDto>>(Messages.NoUserBookFoundByThisPublisherId);
            }
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUsersAllBooksByPublisherId);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByAuthorId(int userId, int authorId)
        {
            var getAuthor = _authorService.GetById(authorId).Data;
            var result = GetBookNotes(_userBookDal.GetBookWithDetails(b =>
                b.UserId == userId && b.AuthorFullName == $"{getAuthor.FirstName} {getAuthor.LastName}"));
            if (result.Count==0)
            {
                return new ErrorDataResult<List<BookForUserDto>>(Messages.NoUserBookFoundByThisAuthorId);
            }
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUsersAllBooksByAuthorId);
        }

        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByAuthorNativeStatue(int userId, bool native)
        {
            var result = GetBookNotes(_userBookDal.GetBookWithDetails(b => b.UserId == userId && b.Native == native));
            if (result.Count==0)
            {
                return new ErrorDataResult<List<BookForUserDto>>(Messages.NoUserBookFoundByThisNativeStatue);
            }
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUserBooksByNativeStatueSuccessfully);
        }

        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByGenreId(int userId, int genreId)
        {
            var getGenre = _genreService.GetById(genreId).Data;
            var result = GetBookNotes(_userBookDal.GetBookWithDetails(b => b.UserId == userId && b.GenreName == getGenre.Name));
            if (result.Count==0)
            {
                return new ErrorDataResult<List<BookForUserDto>>(Messages.NoUserBookFoundByThisGenreId);
            }
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUsersAllBookByGenreIdSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByReadStatue(int userId, bool readStatue)
        {
            var result = GetBookNotes(_userBookDal.GetBookWithDetails(b => b.UserId == userId && b.ReadStatue == readStatue));
            if (result.Count==0)
            {
                return new ErrorDataResult<List<BookForUserDto>>(Messages.NoUserBookFoundByThisReadStatue);
            }
            return new SuccessDataResult<List<BookForUserDto>>(result, Messages.GetUsersAllBookByReadStatueSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<UserBook>> GetAllUserBooks(int userId)
        {
            // error kontrole gerek yok, kullanılan yerde data ya göre kontrol edildi.
            return new SuccessDataResult<List<UserBook>>(_userBookDal.GetAll(u => u.UserId == userId),
                Messages.GetAllUserBookEntitiesSuccessfully);
        }

        [SecuredOperation("user")]
        [ValidationAspect(typeof(UserBookValidator))]
        public IResult Add(UserBook userBook)
        {
            var result = BusinessRules.Run(CheckThisBookAddedUserLibraryBefore(userBook));

            if (result != null)
            {
                return result;
            }
            userBook.Note = CheckNotesForWhiteSpace(userBook.Note);
            _userBookDal.Add(userBook);
            return new SuccessResult(Messages.UserBookAddedSuccessfully);
        }
        [SecuredOperation("user")]
        [ValidationAspect(typeof(UserBookValidator))]
        public IResult Update(UserBook userBook)
        {
            var result = BusinessRules.Run(CheckUserBookUpdatable(userBook));

            if (result != null)
            {
                return result;
            }

            var tryToGetUserBook = _userBookDal.Get(u => u.Id == userBook.Id);
            tryToGetUserBook.UserId = userBook.UserId;
            tryToGetUserBook.BookId = userBook.BookId;
            tryToGetUserBook.ReadStatue = userBook.ReadStatue;
            tryToGetUserBook.Note = CheckNotesForWhiteSpace(userBook.Note);
            _userBookDal.Update(tryToGetUserBook);
            return new SuccessResult(Messages.UserBookUpdatedSuccessfully);
        }
        [SecuredOperation("user")]
        public IResult Delete(UserBook userBook)
        {
            var userBookToDelete = _userBookDal.Get(u => u.Id == userBook.Id);
            if (userBookToDelete==null)
            {
                return new ErrorResult(Messages.CanNotFindUserBook);
            }
            _userBookDal.Delete(userBookToDelete);
            return new SuccessResult(Messages.UserBookDeletedSuccessfully);
        }

        private List<BookForUserDto> GetBookNotes(List<BookForUserDto> bookForUserDtos)
        {
            foreach (BookForUserDto bookForUserDto in bookForUserDtos)
            {
                var getBookNote = _userBookDal.Get(u =>
                    u.UserId == bookForUserDto.UserId && u.BookId == bookForUserDto.BookId);

                bookForUserDto.NoteDetail = getBookNote.Note;
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

        private IResult CheckUserBookUpdatable(UserBook userBook)
        {
            var getBookToUpdate = _userBookDal.Get(u => u.Id == userBook.Id);
            var tryUserBookAddedBefore = _userBookDal.Get(u => u.UserId == userBook.UserId && u.BookId == userBook.BookId);

            if (getBookToUpdate.Id != tryUserBookAddedBefore.Id)
            {
                return new ErrorResult(Messages.UserBookConflict);
            }

            return new SuccessResult();
        }

        private string CheckNotesForWhiteSpace(string userBookNote)
        {
            if (userBookNote != null)
            {
                userBookNote = StringEditorHelper.TrimStartAndFinish(userBookNote);
                if (userBookNote == string.Empty)
                {
                    userBookNote = null;
                }
            }

            return userBookNote;
        }

        
    }
}
