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
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Add(UserOperationClaimWithAttemptingUserIdDto userOperationClaimWithAttemptingUserIdDto)
        {
            UserOperationClaim userOperationClaim = new UserOperationClaim
            {
                UserId = userOperationClaimWithAttemptingUserIdDto.UserId,
                OperationClaimId = userOperationClaimWithAttemptingUserIdDto.OperationClaimId
            };
            var checkIfRoleAddedBeforeAndHasUserAdminRoleOrRoleTryToAddIsAdmin = BusinessRules.Run(CheckIfRoleAddedToUserAlready(userOperationClaim), IsTheRoleAdminAndHasAttemptingUserAdminRole(userOperationClaimWithAttemptingUserIdDto), ThisUserHasAdminRoleAlready(userOperationClaim), ThisUserHasAdminRoleNow(userOperationClaimWithAttemptingUserIdDto.AttemptingUserId,userOperationClaim));
            if (checkIfRoleAddedBeforeAndHasUserAdminRoleOrRoleTryToAddIsAdmin != null)
            {
                return checkIfRoleAddedBeforeAndHasUserAdminRoleOrRoleTryToAddIsAdmin;
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
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Delete(UserOperationClaimWithAttemptingUserIdDto userOperationClaimWithAttemptingUserIdDto)
        {
            UserOperationClaim userOperationClaim = new UserOperationClaim
            {
                Id = userOperationClaimWithAttemptingUserIdDto.Id
            };

            var checkRoleAndAttemptingUserRoleSuitableForDelete = BusinessRules.Run(CanNotDeleteOrUpdateUserRole(userOperationClaim), IsTheRoleAdminAndHasAttemptingUserAdminRole(userOperationClaimWithAttemptingUserIdDto),CheckIfRoleAdminAndSystemHasAnotherAdmin(userOperationClaim));
            if (checkRoleAndAttemptingUserRoleSuitableForDelete != null)
            {
                return checkRoleAndAttemptingUserRoleSuitableForDelete;
            }

            var result = _userOperationClaimDal.Get(u => u.Id == userOperationClaim.Id);
            if (result == null)
            {
                return new ErrorResult(Messages.UserOperationClaimNotFoundById);
            }

            _userOperationClaimDal.Delete(result);
            return new SuccessResult(Messages.UserOperationClaimDeletedSuccessfully);
        }

        [SecuredOperation("admin")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult DeleteClaimFromAllUsersWhenClaimDeleted(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Delete(userOperationClaim);
            return new SuccessResult(); //mesaja gerek yok çağırıldığı yerde mesaj dönüyor.
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

        private IResult CheckIfRoleAdminAndSystemHasAnotherAdmin(UserOperationClaim userOperationClaim)
        {
            var getUserOperaitonClaimFirst = _userOperationClaimDal.Get(u => u.Id == userOperationClaim.Id);
            var getRoleTryToDelete = _operationClaimService.Value.GetById(getUserOperaitonClaimFirst.OperationClaimId).Data;
            var getAdminRole = _operationClaimService.Value.GetByClaimNameIfClaimActive("admin").Data;

            if (getRoleTryToDelete.Id == getAdminRole.Id)
            {
                var systemHasAnotherAdmin = _userOperationClaimDal.GetAll(u =>
                    u.OperationClaimId == getAdminRole.Id && u.UserId != getUserOperaitonClaimFirst.UserId);
                if (systemHasAnotherAdmin.Count == 0)
                {
                    return new ErrorResult(Messages.SystemHasNoAnyOtherAdmin);
                }
            }

            return new SuccessResult();
        }


        private IResult ThisUserHasAdminRoleAlready(UserOperationClaim userOperationClaim)
        {
            var getAdminRoleFirst = _operationClaimService.Value.GetByClaimNameIfClaimActive("admin").Data;
            var getUsersOperationClaimsByUserId = _userOperationClaimDal.GetAll(u => u.UserId == userOperationClaim.UserId);
            var getUserRole = _operationClaimService.Value.GetByClaimNameIfClaimActive("user").Data;
            if (getAdminRoleFirst != null && getUsersOperationClaimsByUserId.Any(u => u.OperationClaimId == getAdminRoleFirst.Id) && userOperationClaim.OperationClaimId != getUserRole.Id)
            {
                return new ErrorResult(Messages.ThisUserHasAdminRoleAlready);
            }

            return new SuccessResult();
        }

        private IResult ThisUserHasAdminRoleNow(int attemptedUserId, UserOperationClaim userOperationClaim)
        {
            // burada user.admin kendine admin rolü eklemek istediğinde hata mesajını alıyor ancak burası hep success döndüğü için mevcuttaki user admin rolü siliniyor. DÜZELT
            var getUsersOperationClaimsByUserId = _userOperationClaimDal.GetAll(u => u.UserId == userOperationClaim.UserId);
            var getAdminRole = _operationClaimService.Value.GetByClaimNameIfClaimActive("admin").Data; //kullanıcıya admin rolü ekleneceği için null check gerekmez.
            var getUserRole = _operationClaimService.Value.GetByClaimNameIfClaimActive("user").Data;

            if (userOperationClaim.OperationClaimId == getAdminRole.Id)
            {
                if (HasAttemptedUserAdminRole(attemptedUserId))
                {
                    foreach (var usersOperationClaim in getUsersOperationClaimsByUserId)
                    {
                        if (usersOperationClaim.OperationClaimId != getUserRole.Id)
                        {
                            _userOperationClaimDal.Delete(usersOperationClaim);
                        }
                    }
                    
                    return new SuccessResult();
                }

                return new ErrorResult(Messages.AdminRoleCanAddOrDeleteAnotherAdminOnly);
            }

            return new SuccessResult();

        }


        private IResult IsTheRoleAdminAndHasAttemptingUserAdminRole(UserOperationClaimWithAttemptingUserIdDto userOperationClaimWithAttemptingUserIdDto)
        {
            var getAdminRole = _operationClaimService.Value.GetByClaimNameIfClaimActive("admin");

            if (userOperationClaimWithAttemptingUserIdDto.OperationClaimId != 0)
            {

                if (userOperationClaimWithAttemptingUserIdDto.OperationClaimId == getAdminRole.Data.Id)
                {
                    if (HasAttemptedUserAdminRole(userOperationClaimWithAttemptingUserIdDto.AttemptingUserId))
                    {
                        return new SuccessResult();
                    }
                    return new ErrorResult(Messages.AdminRoleCanAddOrDeleteAnotherAdminOnly);
                }
            }
            else
            {
                var getUserOperationClaim = _userOperationClaimDal.Get(u => u.Id == userOperationClaimWithAttemptingUserIdDto.Id);

                if (getUserOperationClaim.OperationClaimId == getAdminRole.Data.Id)
                {
                    if (HasAttemptedUserAdminRole(userOperationClaimWithAttemptingUserIdDto.AttemptingUserId))
                    {
                        return new SuccessResult();
                    }
                    return new ErrorResult(Messages.AdminRoleCanAddOrDeleteAnotherAdminOnly);
                }
            }

            return new SuccessResult();

        }

        private bool HasAttemptedUserAdminRole(int attemptedUserId)
        {
            if (_userOperationClaimDal.GetUserClaimDtosByUserId(attemptedUserId).Any(u => u.Name == "admin"))
            {
                return true;
            }

            return false;
        }

    }
}
