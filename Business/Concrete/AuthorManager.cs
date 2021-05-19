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

namespace Business.Concrete
{
    public class AuthorManager : IAuthorService
    {
        private readonly IAuthorDal _authorDal;

        public AuthorManager(IAuthorDal authorDal)
        {
            _authorDal = authorDal;
        }
        [SecuredOperation("admin,author.admin,user")]
        [CacheAspect()]
        public IDataResult<List<Author>> GetAll()
        {
            return new SuccessDataResult<List<Author>>(_authorDal.GetAll(a => a.Active), Messages.GetAllAuthorsSuccessfully);
        }
        [SecuredOperation("admin,author.admin,user")]
        public IDataResult<Author> GetById(int id)
        {
            return new SuccessDataResult<Author>(_authorDal.Get(a => a.Id == id && a.Active), Messages.GetAuthorByIdSuccessfully);
        }
        [SecuredOperation("admin,author.admin")]
        [CacheRemoveAspect("IAuthorService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(AuthorValidator))]
        public IResult Add(Author author)
        {
            var authorExistsAndActiveAlready = BusinessRules.Run(IsAuthorAlreadyExistAndActive(author));

            if (authorExistsAndActiveAlready!=null)
            {
                return authorExistsAndActiveAlready;
            }

            var result = IsAuthorAddedBeforeAndNotActiveNow(author);
            if (result==null)
            {
                author.FirstName = AuthorNameEditorByAuthorNativeStatue(author).FirstName;
                author.LastName = AuthorNameEditorByAuthorNativeStatue(author).LastName;
                author.Active = true;
                _authorDal.Add(author);
            }
            else
            {
                _authorDal.Update(result);
            }
            
            return new SuccessResult(Messages.AuthorAddedSuccessfully);
        }
        [SecuredOperation("admin,author.admin")]
        [CacheRemoveAspect("IAuthorService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(AuthorValidator))]
        public IResult Update(Author author)
        {
            var authorExistsAndActiveAlready = BusinessRules.Run(IsAuthorAlreadyExistAndActive(author));

            if (authorExistsAndActiveAlready!=null)
            {
                return authorExistsAndActiveAlready;
            }

            author.FirstName = AuthorNameEditorByAuthorNativeStatue(author).FirstName;
            author.LastName = AuthorNameEditorByAuthorNativeStatue(author).LastName;
            _authorDal.Update(author);
            return new SuccessResult(Messages.AuthorUpdatedSuccessfully);
        }
        [SecuredOperation("admin,author.admin")]
        [CacheRemoveAspect("IAuthorService.Get")]
        [TransactionScopeAspect]
        public IResult Delete(Author author)
        {
            var authorToDelete = GetById(author.Id).Data;
            authorToDelete.Active = false;
            _authorDal.Update(authorToDelete);
            return new SuccessResult(Messages.AuthorDeletedSuccessfully);
        }

        private Author AuthorNameEditorByAuthorNativeStatue(Author author)
        {
            if (!author.Native)
            {
                author.FirstName =
                    StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToEngLocaleCamelCase(author.FirstName));

                author.LastName =
                    StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToEngLocaleCamelCase(author.LastName));

            }
            else if (author.Native)
            {
                author.FirstName =
                    StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(author.FirstName));

                author.LastName =
                    StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(author.LastName));
            }

            return author;
        }

        private Author IsAuthorAddedBeforeAndNotActiveNow(Author author)
        {
            var nameEditedAuthor = AuthorNameEditorByAuthorNativeStatue(author);

            var authorMakeActiveAgain = _authorDal.Get(a => a.FirstName == nameEditedAuthor.FirstName && a.LastName == nameEditedAuthor.LastName && a.Native==author.Native && a.Active==false);
            if (authorMakeActiveAgain != null)
            {
                authorMakeActiveAgain.Active = true;
                return authorMakeActiveAgain;
            }
            
            return null;

        }

        private IResult IsAuthorAlreadyExistAndActive(Author author)
        {
            var nameEditedAuthor = AuthorNameEditorByAuthorNativeStatue(author);
            var tryToFindAuthor = _authorDal.Get(a =>
                a.FirstName == nameEditedAuthor.FirstName && a.LastName == nameEditedAuthor.LastName &&
                a.Native == author.Native && a.Active);
            if (tryToFindAuthor!=null)
            {
                return new ErrorResult(Messages.AuthorAlreadyAdded);
            }

            return new SuccessResult();
        }
    }
}
