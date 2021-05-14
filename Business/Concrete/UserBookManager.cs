using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
    public class UserBookManager:IUserBookService
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
            
            return new SuccessDataResult<List<BookForUserDto>>(_userBookDal.GetBookWithDetails(b => b.UserId == userId),
                Messages.GetUsersAllBooksSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByNoteIncluded(int userId)
        {
            return new SuccessDataResult<List<BookForUserDto>>(
                _userBookDal.GetBookWithDetails(b => b.UserId == userId && b.NoteDetail != null),
                Messages.GetUsersAllBooksWichHasNoteSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByPublisherId(int userId, int publisherId)
        {
            var getPublisher = _publisherService.GetById(publisherId).Data;
            return new SuccessDataResult<List<BookForUserDto>>(
                _userBookDal.GetBookWithDetails(b => b.UserId == userId && b.PublisherName == getPublisher.Name),Messages.GetUsersAllBooksByPublisherId);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByAuthorId(int userId, int authorId)
        {
            var getAuthor = _authorService.GetById(authorId).Data;
            return new SuccessDataResult<List<BookForUserDto>>(
                _userBookDal.GetBookWithDetails(b =>
                    b.UserId == userId && b.AuthorFullName == $"{getAuthor.FirstName} {getAuthor.LastName}"), Messages.GetUsersAllBooksByAuthorId);
        }

        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByAuthorNativeStatue(int userId, bool native)
        {
            return new SuccessDataResult<List<BookForUserDto>>(_userBookDal.GetBookWithDetails(b => b.Native == native),
                Messages.GetUserBooksByNativeStatueSuccessfully);
        }

        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByGenreId(int userId, int genreId)
        {
            var getGenre = _genreService.GetById(genreId).Data;
            return new SuccessDataResult<List<BookForUserDto>>(
                _userBookDal.GetBookWithDetails(b => b.UserId == userId && b.GenreName == getGenre.Name),
                Messages.GetUsersAllBookByGenreIdSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<List<BookForUserDto>> GetByReadStatue(int userId, bool readStatue)
        {
            return new SuccessDataResult<List<BookForUserDto>>(
                _userBookDal.GetBookWithDetails(b => b.UserId == userId && b.ReadStatue == readStatue),
                Messages.GetUsersAllBookByReadStatueSuccessfully);
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
            _userBookDal.Add(userBook);
            return new SuccessResult(Messages.UserBookAddedSuccessfully);
        }
        [SecuredOperation("user")]
        public IResult Update(UserBook userBook)
        {
            _userBookDal.Update(userBook);
            return new SuccessResult(Messages.UserBookUpdatedSuccessfully);
        }
        [SecuredOperation("admin,user")]
        public IResult Delete(UserBook userBook)
        {
            _userBookDal.Delete(userBook);
            return new SuccessResult(Messages.UserBookDeletedSuccessfully);
        }

    }
}
