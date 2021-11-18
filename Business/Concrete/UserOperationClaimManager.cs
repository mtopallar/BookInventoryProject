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
using DataAccess.Abstract;
using Entities.DTOs;

namespace Business.Concrete
{
    public class UserOperationClaimManager:IUserOperationClaimService
    {
        private readonly IUserOperationClaimDal _userOperationClaimDal;

        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal)
        {
            _userOperationClaimDal = userOperationClaimDal;
        }
        
        [SecuredOperation("admin,user.admin")]
        [ValidationAspect(typeof(UserOperationClaimValidator))]
        public IResult Add(UserOperationClaim userOperationClaim)
        {
            var checkIfRoleAddedBefore = BusinessRules.Run(CheckIfRoleAddedToUserAlready(userOperationClaim));
            if (checkIfRoleAddedBefore!=null)
            {
                return checkIfRoleAddedBefore;
            }
            _userOperationClaimDal.Add(userOperationClaim);
            return new SuccessResult(Messages.UserOperationClaimAddedSuccessfully);
        }
        // AddUserRoleForUsers SecuredOperation Olamaz. user rolünü burası atayacak.
        public IResult AddUserRoleForUsers(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Add(userOperationClaim);
            return new SuccessResult(Messages.UserRoleSuccessfullyAddedToUser);
        }
        [SecuredOperation("admin,user.admin")]
        [ValidationAspect(typeof(UserOperationClaimValidator))]
        public IResult Update(UserOperationClaim userOperationClaim)
        {
            var checkIfRoleAddedBefore = BusinessRules.Run(CheckIfRoleAddedToUserAlready(userOperationClaim));
            if (checkIfRoleAddedBefore!=null)
            {
                return checkIfRoleAddedBefore;
            }

            var tryToGetUserOperationClaim = _userOperationClaimDal.Get(u => u.Id == userOperationClaim.Id);
            tryToGetUserOperationClaim.OperationClaimId = userOperationClaim.OperationClaimId;
            tryToGetUserOperationClaim.UserId = userOperationClaim.UserId;
            _userOperationClaimDal.Update(tryToGetUserOperationClaim);
            return new SuccessResult(Messages.UserOperationClaimUpdatedSuccessfully);
        }
        [SecuredOperation("admin,user.admin")]
        public IResult Delete(UserOperationClaim userOperationClaim)
        {
            var result = _userOperationClaimDal.Get(u => u.Id == userOperationClaim.Id);
            if (result==null)
            {
                return new ErrorResult(Messages.UserOperationClaimNotFoundByIdForDelete);
            }
            _userOperationClaimDal.Delete(result);
            return new SuccessResult(Messages.UserOperationClaimDeletedSuccessfully);
        }
        [SecuredOperation("user")]
        public IResult DeleteForUsersOwnClaim(int userId)
        {
            //error kontrolüne gerek yok en az 1 user rolü sisteme otomatik eklenmiş olacak.
            var userRoles = _userOperationClaimDal.GetAll(u=>u.UserId==userId);
            foreach (UserOperationClaim userOperationClaim in userRoles)
            {
                _userOperationClaimDal.Delete(userOperationClaim);
            }
            return new SuccessResult(Messages.UserOperationClaimDeletedSuccessfullyByUser);
        }

        private IResult CheckIfRoleAddedToUserAlready(UserOperationClaim userOperationClaim)
        {
            var tryToFindClaim = _userOperationClaimDal.Get(u =>
                u.UserId == userOperationClaim.UserId && u.OperationClaimId == userOperationClaim.OperationClaimId);
            if (tryToFindClaim!=null)
            {
                return new ErrorResult(Messages.UserHasTheRoleAlready);
            }

            return new SuccessResult();
        }
    }
}
