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
using Core.Utilities.StringEditor;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class AuthorManager:IAuthorService
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
            return new SuccessDataResult<List<Author>>(_authorDal.GetAll(), Messages.GetAllAuthorsSuccessfully);
        }
        [SecuredOperation("admin,author.admin,user")]
        public IDataResult<Author> GetById(int id)
        {
            return new SuccessDataResult<Author>(_authorDal.Get(a => a.Id == id), Messages.GetAuthorByIdSuccessfully);
        }
        [SecuredOperation("admin,author.admin")]
        [CacheRemoveAspect("IAuthorService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(AuthorValidator))]
        public IResult Add(Author author)
        {
            if (!author.Native)
            {
                author.FirstName =
                    StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToEngLocaleCamelCase(author.FirstName));

                author.LastName =
                    StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToEngLocaleCamelCase(author.LastName));

            }
            else
            {
                author.FirstName =
                    StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(author.FirstName));

                author.LastName =
                    StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(author.LastName));
            }
            _authorDal.Add(author);
            return new SuccessResult(Messages.AuthorAddedSuccessfully);
        }
        [SecuredOperation("admin,author.admin")]
        [CacheRemoveAspect("IAuthorService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(AuthorValidator))]
        public IResult Update(Author author)
        {
            if (!author.Native)
            {
                author.FirstName =
                    StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToEngLocaleCamelCase(author.FirstName));

                author.LastName =
                    StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToEngLocaleCamelCase(author.LastName));

            }
            else
            {
                author.FirstName =
                    StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(author.FirstName));

                author.LastName =
                    StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(author.LastName));
            }
            _authorDal.Update(author);
            return new SuccessResult(Messages.AuthorUpdatedSuccessfully);
        }
        [SecuredOperation("admin,author.admin")]
        [CacheRemoveAspect("IAuthorService.Get")]
        [TransactionScopeAspect]
        public IResult Delete(Author author)
        {
            _authorDal.Delete(author);
            return new SuccessResult(Messages.AuthorDeletedSuccessfully);
        }
    }
}
