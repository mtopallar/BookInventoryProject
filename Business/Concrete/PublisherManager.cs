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
    public class PublisherManager : IPublisherService
    {
        private readonly IPublisherDal _publisherDal;

        public PublisherManager(IPublisherDal publisherDal)
        {
            _publisherDal = publisherDal;
        }
        [SecuredOperation("user")]
        [CacheAspect()]
        public IDataResult<List<Publisher>> GetAll()
        {
            var result = _publisherDal.GetAll(p => p.Active);
            if (result.Count==0)
            {
                return new ErrorDataResult<List<Publisher>>(Messages.NoActivePubliserFound);
            }
            return new SuccessDataResult<List<Publisher>>(result, Messages.GetAllPublishersSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<Publisher> GetById(int id)
        {
            var result = _publisherDal.Get(p => p.Id == id && p.Active);
            if (result==null)
            {
                return new ErrorDataResult<Publisher>(Messages.PublisherWrongIdOrPublisherNotActive);
            }
            return new SuccessDataResult<Publisher>(result, Messages.GetPublisherByIdSuccessfully);
        }
        [SecuredOperation("admin,publisher.admin")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(PublisherValidator))]
        [CacheRemoveAspect("IPublisherService.Get")]
        public IResult Add(Publisher publisher)
        {
            var isPublisherAlreadyExistAndActive = BusinessRules.Run(IsPublisherAlreadyExistAndActive(publisher));
            if (isPublisherAlreadyExistAndActive != null)
            {
                return isPublisherAlreadyExistAndActive;
            }

            var result = IsPublisherAddedBeforeAndNotActiveNow(publisher);

            if (result == null)
            {
                publisher.Name = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(publisher.Name));
                publisher.Active = true;
                _publisherDal.Add(publisher);
            }
            else
            {
                _publisherDal.Update(result);
            }

            return new SuccessResult(Messages.PublisherAddedSuccessfully);
        }
        [SecuredOperation("admin,publisher.admin")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(PublisherValidator))]
        [CacheRemoveAspect("IPublisherService.Get")]
        public IResult Update(Publisher publisher)
        {
            var checkNewPublisherBeforeUpdateIsPublisherAddedBeforeAndActive = BusinessRules.Run(IsPublisherAlreadyExistAndActive(publisher));
            if (checkNewPublisherBeforeUpdateIsPublisherAddedBeforeAndActive!=null)
            {
                return checkNewPublisherBeforeUpdateIsPublisherAddedBeforeAndActive;
            }

            var checkNewPublisherBeforeUpdateIsPublisherAddedBeforeAndNotActive = IsPublisherAddedBeforeAndNotActiveNow(publisher);
            if (checkNewPublisherBeforeUpdateIsPublisherAddedBeforeAndNotActive!=null)
            {
                _publisherDal.Update(checkNewPublisherBeforeUpdateIsPublisherAddedBeforeAndNotActive);
                return new SuccessResult(Messages.PublisherActivatedNotUpdated);
            }

            var tryToGetPublisher = GetById(publisher.Id);
            if (!tryToGetPublisher.Success)
            {
                return new ErrorResult(tryToGetPublisher.Message);
            }
            tryToGetPublisher.Data.Name = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(publisher.Name));
            _publisherDal.Update(tryToGetPublisher.Data);
            return new SuccessResult(Messages.UpdatedPublisherSuccessfully);
        }

        [SecuredOperation("admin,publisher.admin")]
        [TransactionScopeAspect]
        [CacheRemoveAspect("IPublisherService.Get")]
        public IResult Delete(Publisher publisher)
        {
            var publishertoDelete = GetById(publisher.Id);
            if (!publishertoDelete.Success)
            {
                return new ErrorResult(publishertoDelete.Message);
            }
            publishertoDelete.Data.Active = false;
            _publisherDal.Update(publishertoDelete.Data);
            return new SuccessResult(Messages.DeletePublisherSuccessfully);
        }

        private Publisher IsPublisherAddedBeforeAndNotActiveNow(Publisher publisher)
        {
            var nameEditedPublisher =
                StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(publisher.Name));
            var tryToGetPublisher = _publisherDal.Get(p => p.Name == nameEditedPublisher && p.Active == false);

            if (tryToGetPublisher != null)
            {
                tryToGetPublisher.Active = true;
                return tryToGetPublisher;
            }

            return null;
        }

        private IResult IsPublisherAlreadyExistAndActive(Publisher publisher)
        {
            var nameEditedPublisher =
                StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(publisher.Name));
            var tryToGetPublisher = _publisherDal.Get(p => p.Name == nameEditedPublisher && p.Active);

            if (tryToGetPublisher != null)
            {
                return new ErrorResult(Messages.PublisherAlreadyAdded);
            }

            return new SuccessResult();
        }

    }
}
