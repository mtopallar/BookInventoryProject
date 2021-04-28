using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
   public class BookManager:IBookService
   {
       private readonly IBookDal _bookDal;

       public BookManager(IBookDal bookDal)
       {
           _bookDal = bookDal;
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
        [SecuredOperation("admin,book.admin")]
        [CacheRemoveAspect("IBookService.Get")]
        [TransactionScopeAspect]
        public IResult Add(Book book)
        {
            _bookDal.Add(book);
            return new SuccessResult(Messages.BookAddedSuccessfully);
        }
        [SecuredOperation("admin,book.admin")]
        [CacheRemoveAspect("IBookService.Get")]
        [TransactionScopeAspect]
        public IResult Update(Book book)
        {
            _bookDal.Update(book);
            return new SuccessResult(Messages.BookUpdatedSuccessfully);
        }
        //[SecuredOperation("admin,book.admin")]
        //[CacheRemoveAspect("IBookService.Get")]
        //[TransactionScopeAspect]
        //public IResult Delete(Book book)
        //{
        //    _bookDal.Delete(book);
        //    return new SuccessResult(Messages.BookDeletedSuccessfully);
        //}
    }
}
