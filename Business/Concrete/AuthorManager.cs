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
        [SecuredOperation("user")]
        [CacheAspect()]
        public IDataResult<List<Author>> GetAll()
        {
            var checkIfNoActiveAuthors = _authorDal.GetAll(a => a.Active);
            if (checkIfNoActiveAuthors.Count == 0)
            {
                return new ErrorDataResult<List<Author>>(Messages.NoActiveAuthorsFound);
            }
            return new SuccessDataResult<List<Author>>(checkIfNoActiveAuthors, Messages.GetAllAuthorsSuccessfully);
        }

        [SecuredOperation("user")]
        [CacheAspect()]
        public IDataResult<List<Author>> GetAllRegardlessOfActiveStatue()
        {
            var getAuthors = _authorDal.GetAll();
            if (getAuthors.Count ==0)
            {
                return new ErrorDataResult<List<Author>>(Messages.NoAnyAuthorRegardlessofActiveStatue);
            }

            return new SuccessDataResult<List<Author>>(getAuthors, Messages.GetAuthorRegardlessofActiveStatueSuccessfully);
        }

        [SecuredOperation("user")]
        public IDataResult<Author> GetById(int id)
        {
            var tryGetAuthorByIdIfAuthorActive = _authorDal.Get(a => a.Id == id && a.Active);
            if (tryGetAuthorByIdIfAuthorActive == null)
            {
                return new ErrorDataResult<Author>(Messages.CanNotFindActiveAuthor);
            }
            return new SuccessDataResult<Author>(_authorDal.Get(a => a.Id == id && a.Active), Messages.GetAuthorByIdSuccessfully);
        }
        [SecuredOperation("admin,author.admin")]
        [CacheRemoveAspect("IAuthorService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(AuthorValidator))]
        public IResult Add(Author author)
        {
            var authorExistsAndActiveAlready = BusinessRules.Run(IsAuthorAlreadyExistAndActive(author));

            if (authorExistsAndActiveAlready != null)
            {
                return authorExistsAndActiveAlready;
            }

            var result = IsAuthorAddedBeforeAndNotActiveNow(author);
            if (result == null)
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
            var checkNewAuthorBeforeUpdateIsAuthorAddedBeforeAndActive = BusinessRules.Run(IsAuthorAlreadyExistAndActive(author));
            if (checkNewAuthorBeforeUpdateIsAuthorAddedBeforeAndActive != null)
            {
                return checkNewAuthorBeforeUpdateIsAuthorAddedBeforeAndActive; //BusinessRules dan ErrorResult dönmüş demektir.
            }

            var checkNewAuthorBeforeUpdateIsAuthorAddedBeforeAndNotActive = IsAuthorAddedBeforeAndNotActiveNow(author);
            if (checkNewAuthorBeforeUpdateIsAuthorAddedBeforeAndNotActive!=null)
            {
                _authorDal.Update(checkNewAuthorBeforeUpdateIsAuthorAddedBeforeAndNotActive);
                return new SuccessResult(Messages.AuthorActivatedNotUpdated);
            }
            var tryToGetAuthor = GetById(author.Id);
            if (!tryToGetAuthor.Success)
            {
                return new ErrorResult(tryToGetAuthor.Message);
            }
            tryToGetAuthor.Data.FirstName = AuthorNameEditorByAuthorNativeStatue(author).FirstName;
            tryToGetAuthor.Data.LastName = AuthorNameEditorByAuthorNativeStatue(author).LastName;
            tryToGetAuthor.Data.Native = author.Native;
            _authorDal.Update(tryToGetAuthor.Data);
            return new SuccessResult(Messages.AuthorUpdatedSuccessfully);
        }
        [SecuredOperation("admin,author.admin")]
        [CacheRemoveAspect("IAuthorService.Get")]
        [TransactionScopeAspect]
        public IResult Delete(Author author)
        {
            var authorToDelete = GetById(author.Id);
            if (!authorToDelete.Success)
            {
                return new ErrorResult(authorToDelete.Message);
            }
            authorToDelete.Data.Active = false;
            _authorDal.Update(authorToDelete.Data);
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
            //author un  id değerine göre author getirilip active değeri false ise kontrolü de yapılabilir.
            var nameEditedAuthor = AuthorNameEditorByAuthorNativeStatue(author);

            var authorMakeActiveAgain = _authorDal.Get(a => a.FirstName == nameEditedAuthor.FirstName && a.LastName == nameEditedAuthor.LastName && a.Native == author.Native && a.Active == false);
            if (authorMakeActiveAgain != null)
            {
                authorMakeActiveAgain.Active = true;
                return authorMakeActiveAgain;
            }

            return null;

        }

        private IResult IsAuthorAlreadyExistAndActive(Author author)
        {
            //author un  id değerine göre author getirilip active değeri false ise kontrolü de yapılabilir.
            var nameEditedAuthor = AuthorNameEditorByAuthorNativeStatue(author);
            var tryToFindAuthor = _authorDal.Get(a =>
                a.FirstName == nameEditedAuthor.FirstName && a.LastName == nameEditedAuthor.LastName &&
                a.Native == author.Native && a.Active);
            if (tryToFindAuthor != null)
            {
                return new ErrorResult(Messages.AuthorAlreadyAdded);
            }

            return new SuccessResult();
        }
    }
}
