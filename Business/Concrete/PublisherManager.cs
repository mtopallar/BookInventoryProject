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
    public class PublisherManager:IPublisherService
    {
        private readonly IPublisherDal _publisherDal;

        public PublisherManager(IPublisherDal publisherDal)
        {
            _publisherDal = publisherDal;
        }
        [SecuredOperation("admin,publisher.admin")]
        [CacheAspect()]
        public IDataResult<List<Publisher>> GetAll()
        {
            return new SuccessDataResult<List<Publisher>>(_publisherDal.GetAll(),
                Messages.GetAllPublishersSuccessfully);
        }
        [SecuredOperation("admin,publisher.admin")]
        public IDataResult<Publisher> GetById(int id)
        {
            return new SuccessDataResult<Publisher>(_publisherDal.Get(p => p.Id == id),
                Messages.GetPublisherByIdSuccessfully);
        }
        [SecuredOperation("admin,publisher.admin")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(PublisherValidator))]
        [CacheRemoveAspect("IPublisherService.Get")]
        public IResult Add(Publisher publisher)
        {
            _publisherDal.Add(publisher);
            return new SuccessResult(Messages.PublisherAddedSuccessfully);
        }
        [SecuredOperation("admin,publisher.admin")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(PublisherValidator))]
        [CacheRemoveAspect("IPublisherService.Get")]
        public IResult Update(Publisher publisher)
        {
            _publisherDal.Update(publisher);
            return new SuccessResult(Messages.UpdatedPublisherSuccessfully);
        }
        [SecuredOperation("admin,publisher.admin")]
        [TransactionScopeAspect]
        [CacheRemoveAspect("IPublisherService.Get")]
        public IResult Delete(Publisher publisher)
        {
            _publisherDal.Delete(publisher);
            return new SuccessResult(Messages.DeletePublisherSuccessfully);
        }
    }
}
