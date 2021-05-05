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

namespace Business.Concrete
{
    public class NationalityManager:INationalityService
    {
        private readonly INationalityDal _nationalityDal;

        public NationalityManager(INationalityDal nationalityDal)
        {
            _nationalityDal = nationalityDal;
        }
        [SecuredOperation("admin,nationality.admin")]
        [CacheAspect()]
        public IDataResult<List<Nationality>> GetAll()
        {
            return new SuccessDataResult<List<Nationality>>(_nationalityDal.GetAll(),
                Messages.GetAllNationalitySuccessfull);
        }
        [SecuredOperation("admin,nationality.admin")]
        public IDataResult<Nationality> GetById(int id)
        {
            return new SuccessDataResult<Nationality>(_nationalityDal.Get(n => n.Id == id),
                Messages.GetNationalityByIdSuccessfully);
        }
        [SecuredOperation("admin,nationality.admin")]
        [CacheRemoveAspect("INationalityService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(NationalityValidator))]
        public IResult Add(Nationality nationality)
        {
            _nationalityDal.Add(nationality);
            return new SuccessResult(Messages.NationalityAddedSuccessfully);
        }
        [SecuredOperation("admin,nationality.admin")]
        [CacheRemoveAspect("INationalityService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(NationalityValidator))]
        public IResult Update(Nationality nationality)
        {
            _nationalityDal.Update(nationality);
            return new SuccessResult(Messages.NationalityUpdatedSuccessfully);
        }
        [SecuredOperation("admin,nationality.admin")]
        [TransactionScopeAspect]
        [CacheRemoveAspect("INationalityService.Get")]
        public IResult Delete(Nationality nationality)
        {
            _nationalityDal.Delete(nationality);
            return new SuccessResult(Messages.NationalityDeletedSuccessfully);
        }
    }
}
