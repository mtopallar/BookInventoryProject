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
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.DTOs;

namespace Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimDal _userOperationClaimDal;
        private readonly Lazy<IOperationClaimService> _operationClaimService;

        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal, Lazy<IOperationClaimService> operationClaimService)
        {
            _userOperationClaimDal = userOperationClaimDal;
            _operationClaimService = operationClaimService;
        }

        public IDataResult<List<UserOperationClaim>> GetAll() // iç metod olacak. secured operations a gerek yok.
        {
            var result = _userOperationClaimDal.GetAll();
            if (result.Count == 0)
            {
                // buraya düşmemeli, önlem amaçlı.
                return new ErrorDataResult<List<UserOperationClaim>>(Messages.NoAnyUserOperationCalimsInSystem);
            }

            return new SuccessDataResult<List<UserOperationClaim>>(result, Messages.GetAllUserOperationClaimsSuccessfully);
        }

        public IDataResult<List<UserOperationClaim>> GetByUserId(int userId) // iç metod olacak. secured operations a gerek yok.
        {
            var result = _userOperationClaimDal.GetAll(u => u.UserId == userId);
            if (result.Count == 0)
            {
                return new ErrorDataResult<List<UserOperationClaim>>(Messages.GetUserOperaitonClaimsByUserIdError);
            }

            return new SuccessDataResult<List<UserOperationClaim>>(result, Messages.GetUserOperaitonClaimsByIdSuccessfully);
        }

        [SecuredOperation("admin,user.admin")]
        public IDataResult<List<UserOperationClaimDto>> GetUserClaimDtosByUserId(int userId)
        {
            var result = _userOperationClaimDal.GetUserClaimDtosByUserId(userId);
            if (result == null)
            {
                // Bu kısım aslında sigorta gibi normalde sistemburaya düşmemeli.
                return new ErrorDataResult<List<UserOperationClaimDto>>(Messages.UserHasNoActiveRole);
            }

            return new SuccessDataResult<List<UserOperationClaimDto>>(result, Messages.GetUserRoleDtosSuccessfully);
        }


        [SecuredOperation("admin")] // apide yer almayacak. Operation Claim Manager Delete için kullanacak. Çağırılacağı yer sadece admin yetkisinde.
        public IDataResult<List<UserOperationClaim>> GetByClaimId(int operationClaimId)
        {
            var result = _userOperationClaimDal.GetAll(u => u.OperationClaimId == operationClaimId);
            if (result == null)
            {
                return new ErrorDataResult<List<UserOperationClaim>>(Messages.NotFindAnyClaimByThisId);
            }

            return new SuccessDataResult<List<UserOperationClaim>>(result, Messages.ClaimsListedByClaimId);
        }

        [SecuredOperation("admin,user.admin")]
        [ValidationAspect(typeof(UserOperationClaimValidator))]
        [CacheRemoveAspect("IUserOperationClaimService.Get")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Add(UserOperationClaim userOperationClaim)
        {
            var checkIfRoleAddedBefore = BusinessRules.Run(CheckIfRoleAddedToUserAlready(userOperationClaim));
            if (checkIfRoleAddedBefore != null)
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
        [CacheRemoveAspect("IUserOperationClaimService.Get")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Update(UserOperationClaim userOperationClaim)
        {
            var checkIfRoleAddedBefore = BusinessRules.Run(CheckIfRoleAddedToUserAlready(userOperationClaim), CanNotDeleteOrUpdateUserRole(userOperationClaim));
            if (checkIfRoleAddedBefore != null)
            {
                return checkIfRoleAddedBefore;
            }

            var tryToGetUserOperationClaim = _userOperationClaimDal.Get(u => u.Id == userOperationClaim.Id);
            if (tryToGetUserOperationClaim == null)
            {
                return new ErrorResult(Messages.UserOperationClaimNotFoundById);
            }
            tryToGetUserOperationClaim.OperationClaimId = userOperationClaim.OperationClaimId;
            tryToGetUserOperationClaim.UserId = userOperationClaim.UserId;
            _userOperationClaimDal.Update(tryToGetUserOperationClaim);
            return new SuccessResult(Messages.UserOperationClaimUpdatedSuccessfully);
        }
        [SecuredOperation("admin,user.admin")]
        [CacheRemoveAspect("IUserOperationClaimService.Get")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Delete(UserOperationClaim userOperationClaim)
        {
            var checkRoleSuitableForDelete = BusinessRules.Run(CanNotDeleteOrUpdateUserRole(userOperationClaim),
                CheckIfRoleAdminAndSystemHasAnotherAdminOrUserAdmin(userOperationClaim), CheckIfRoleUserAdminAndSystemHasNoAnotherAdminOrUserAdmin(userOperationClaim));
            if (checkRoleSuitableForDelete != null)
            {
                return checkRoleSuitableForDelete;
            }
            var result = _userOperationClaimDal.Get(u => u.Id == userOperationClaim.Id);
            if (result == null)
            {
                return new ErrorResult(Messages.UserOperationClaimNotFoundById);
            }
            //buraya user rolünün silinmesini engelleyen ve sistemde başka bir admin yoksa admin rolünün silinmesini engelleyen bir metod yaz.
            _userOperationClaimDal.Delete(result);
            return new SuccessResult(Messages.UserOperationClaimDeletedSuccessfully);
        }
        [SecuredOperation("user")]
        public IResult DeleteForUsersOwnClaim(int userId)
        {
            //error kontrolüne gerek yok en az 1 user rolü sisteme otomatik eklenmiş olacak.
            var userRoles = _userOperationClaimDal.GetAll(u => u.UserId == userId);
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
            if (tryToFindClaim != null)
            {
                return new ErrorResult(Messages.UserHasTheRoleAlready);
            }

            return new SuccessResult();
        }

        private IResult CanNotDeleteOrUpdateUserRole(UserOperationClaim userOperationClaim)
        {
            var getUserOperaitonClaimFirst = _userOperationClaimDal.Get(u => u.Id == userOperationClaim.Id);
            var getRoleTryToDelete = _operationClaimService.Value.GetById(getUserOperaitonClaimFirst.OperationClaimId).Data;
            if (getRoleTryToDelete.Name == "user")
            {
                return new ErrorResult(Messages.TheRoleUserCanNotDeleteOrUpdate);
            }

            return new SuccessResult();

        }

        private IResult CheckIfRoleAdminAndSystemHasAnotherAdminOrUserAdmin(UserOperationClaim userOperationClaim)
        {
            var getUserOperaitonClaimFirst = _userOperationClaimDal.Get(u => u.Id == userOperationClaim.Id);
            var getRoleTryToDelete = _operationClaimService.Value.GetById(getUserOperaitonClaimFirst.OperationClaimId).Data;
            var getUserAdminRole = _operationClaimService.Value.GetByClaimNameIfClaimActive("user.admin").Data;
            if (getRoleTryToDelete.Name == "admin")
            {
                if (getUserAdminRole != null)
                {
                    var systemHasAnotherAdminOrUserAdmin = _userOperationClaimDal.GetAll(u =>
                     u.OperationClaimId == getUserOperaitonClaimFirst.OperationClaimId && u.UserId != getUserOperaitonClaimFirst.UserId || u.OperationClaimId == getUserAdminRole.Id && u.UserId != getUserOperaitonClaimFirst.UserId);
                    if (systemHasAnotherAdminOrUserAdmin.Count == 0)
                    {
                        return new ErrorResult(Messages.SystemHasNoAnyOtherAdminOrUserAdmin);
                    }
                }
                else
                {
                    var systemHasAnotherAdmin = _userOperationClaimDal.GetAll(u =>
                        u.OperationClaimId == getUserOperaitonClaimFirst.OperationClaimId && u.UserId != getUserOperaitonClaimFirst.UserId);
                    if (systemHasAnotherAdmin.Count == 0)
                    {
                        return new ErrorResult(Messages.SystemHasNoAnyOtherAdmin);
                    }
                }

            }

            return new SuccessResult();
        }

        private IResult CheckIfRoleUserAdminAndSystemHasNoAnotherAdminOrUserAdmin(UserOperationClaim userOperationClaim)
        {
            var getUserOperaitonClaimFirst = _userOperationClaimDal.Get(u => u.Id == userOperationClaim.Id);
            var getRoleTryToDelete = _operationClaimService.Value.GetById(getUserOperaitonClaimFirst.OperationClaimId).Data;
            var getAdminRole = _operationClaimService.Value.GetByClaimNameIfClaimActive("admin").Data;
            if (getRoleTryToDelete.Name == "user.admin")
            {
                if (getAdminRole != null)
                {
                    var systemHasAnotherAdminOrUserAdmin = _userOperationClaimDal.GetAll(u=>u.OperationClaimId == getUserOperaitonClaimFirst.OperationClaimId && u.UserId != getUserOperaitonClaimFirst.UserId || u.OperationClaimId == getAdminRole.Id && u.UserId != getUserOperaitonClaimFirst.UserId);
                    if (systemHasAnotherAdminOrUserAdmin.Count == 0)
                    {
                        return new ErrorResult(Messages.SystemHasNoAnyOtherAdminOrUserAdmin);
                    }
                }
                else
                {
                    var systemHasAnotherUserAdmin = _userOperationClaimDal.GetAll(u =>
                        u.OperationClaimId == getUserOperaitonClaimFirst.OperationClaimId && u.UserId != getUserOperaitonClaimFirst.UserId);
                    if (systemHasAnotherUserAdmin.Count == 0)
                    {
                        return new ErrorResult(Messages.SystemHasNoAnyOtherUserAdmin);
                    }
                }
            }
            return new SuccessResult();
        }


    }
}
