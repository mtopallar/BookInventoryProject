using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.StringEditor;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.DTOs;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IUserBookService _userBookService;

        public UserManager(IUserDal userDal, IOperationClaimService operationClaimService, IUserOperationClaimService userOperationClaimService, IUserBookService userBookService)
        {
            _userDal = userDal;
            _operationClaimService = operationClaimService;
            _userOperationClaimService = userOperationClaimService;
            _userBookService = userBookService;
        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetUserClaims(user),
                Messages.GetUsersAllClaimsSuccessfully);
        }
        [SecuredOperation("admin,user.admin")]
        [CacheAspect()]
        public IDataResult<List<UserWithDetailsAndRolesDto>> GetAllUserDetailsWithRoles()
        {
            var usersDetailsWithoutRoleList = UserToUserWithDetailsAndRolesDto();
            foreach (var userWithDetailsAndRolesDto in usersDetailsWithoutRoleList)
            {
                var usersRoles = GetClaims(new User { Id = userWithDetailsAndRolesDto.UserId }).Data;
                foreach (var userOperationClaim in usersRoles)
                {
                    userWithDetailsAndRolesDto.UserRoleNames.Add(userOperationClaim.Name);
                }
            }

            return new SuccessDataResult<List<UserWithDetailsAndRolesDto>>(usersDetailsWithoutRoleList, Messages.GetAllUserDetailsWitrRolesSuccessfully);
        }
        [SecuredOperation("user")]
        public IDataResult<UserWithDetailsAndRolesDto> GetUserDetailsWithRolesByUserId(int userId)
        {
            var userDetailWithoutRoleList = UserToUserWithDetailsAndRolesDto(userId).Single();
            var userRoleNameList = GetClaims(new User { Id = userId }).Data;

            foreach (var roleNames in userRoleNameList)
            {
                userDetailWithoutRoleList.UserRoleNames.Add(roleNames.Name);
            }
            return new SuccessDataResult<UserWithDetailsAndRolesDto>(userDetailWithoutRoleList, Messages.GetUserDetailsWithRolesByUserIdSuccessfully);
        }
        [SecuredOperation("admin,user.admin")]
        [CacheAspect()]
        public IDataResult<List<User>> GetAll()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetAll(), Messages.GetAllUsersSuccessfully);
        }

        public IDataResult<User> GetByMail(string email)
        {
            var currentMail = _userDal.Get(u => u.Email == email);
            if (currentMail == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFoundByThisMail);
            }
            return new SuccessDataResult<User>(currentMail, Messages.GetUserByEmailSuccessfully);
        }
        [SecuredOperation("admin,user.admin")]
        public IDataResult<User> GetById(int id)
        {
            return new SuccessDataResult<User>(_userDal.Get(u => u.Id == id), Messages.GetUserByIdSuccessfully);
        }
        [ValidationAspect(typeof(UserValidator))]
        [CacheRemoveAspect("IUserService.Get")]
        [TransactionScopeAspect]
        public IResult Add(User user)
        {
            var userRoleName = "user";
            var checkUserRoleBeforeUserAdded = _operationClaimService.GetByClaimNameIfClaimActive(userRoleName);
            user.FirstName = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(user.FirstName));
            user.LastName = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(user.LastName));
            if (checkUserRoleBeforeUserAdded.Success)
            {
                _userDal.Add(user);
                AddUserRoleIfNotExist(user);
                return new SuccessResult(Messages.UserAddedSuccessfully);
            }

            return new ErrorResult(checkUserRoleBeforeUserAdded.Message);
        }
        [SecuredOperation("admin,user.admin,user")]
        [ValidationAspect(typeof(UserForUpdateValidator))]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Update(UserForUpdateDto userForUpdateDto)
        {
            User user = null;
            var checkNewMailIsExists = CheckIfNewMailExists(userForUpdateDto.Email);
            var isItOk = UpdateUserWithPasswordSaltAndHash(userForUpdateDto, ref user);
            if (checkNewMailIsExists.Success)
            {
                if (isItOk.Success)
                {
                    user.FirstName = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(user.FirstName));
                    user.LastName = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(user.LastName));
                    _userDal.Update(user);
                    return new SuccessResult(Messages.UserUpdatedSuccessfully);
                }

                return new ErrorResult(isItOk.Message);
            }

            return checkNewMailIsExists;
        }
        [SecuredOperation("admin,user.admin")]
        [CacheRemoveAspect("IUserService.Get")]
        [TransactionScopeAspect]
        public IResult DeleteForAdmin(int userId)
        {
            var userToDelete = GetById(userId).Data;
            if (DeleteUserBooks(userId).Success)
            {
                if (DeleteUserFromUserOperationClaims(userId).Success)
                {
                    _userDal.Delete(userToDelete);
                }
            }
            return new SuccessResult(Messages.UserAndUsersBooksDeletedSuccessfullyByAdmin);
        }
        [SecuredOperation("user")]
        [ValidationAspect(typeof(UserForDeleteValidator))]
        [CacheRemoveAspect("IUserService.Get")]
        [TransactionScopeAspect]
        public IResult DeleteForUser(UserForDeleteDto userForDeleteDto)
        {
            if (UserCurrentPasswordChecker(userForDeleteDto.CurrentPassword, userForDeleteDto.UserId, out var existUser))
            {
                if (DeleteUserBooks(userForDeleteDto.UserId).Success)
                {
                    if (DeleteUserFromUserOperationClaims(userForDeleteDto.UserId).Success)
                    {
                        _userDal.Delete(existUser);
                        return new SuccessResult(Messages.UserAndUsersBooksAndUserClaimsDeletedSuccessfullyByUser);
                    }
                }
            }
            return new ErrorResult(Messages.CurrentUserPasswordError);
        }

        private void AddUserRoleIfNotExist(User user)
        {
            var claimName = "user";
            var getClaimUser = _operationClaimService.GetByClaimNameIfClaimActive(claimName).Data;
            _userOperationClaimService.AddUserRoleForUsers(new UserOperationClaim { UserId = user.Id, OperationClaimId = getClaimUser.Id });
        }

        private IResult UpdateUserWithPasswordSaltAndHash(UserForUpdateDto updateUserDto, ref User user)
        {
            if (UserCurrentPasswordChecker(updateUserDto.CurrentPassword, updateUserDto.UserId, out var existUser))
            {
                HashingHelper.CreatePasswordHash(updateUserDto.NewPassword, out var passwordHash, out var passwordSalt);
                user = new User
                {
                    Id = existUser.Id,
                    FirstName = updateUserDto.FirstName,
                    LastName = updateUserDto.LastName,
                    Email = updateUserDto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                return new SuccessResult();
            }

            return new ErrorResult(Messages.CurrentUserPasswordError);
        }

        private IResult CheckIfNewMailExists(string email)
        {
            var checkNewMailExists = GetByMail(email);
            if (checkNewMailExists.Success)
            {
                return new ErrorResult(Messages.NewEmailAlreadyExists);
            }

            return new SuccessResult();
        }

        private IResult DeleteUserBooks(int userId)
        {
            var usersBookList = _userBookService.GetAllUserBooks(userId).Data;
            if (usersBookList.Count != 0)
            {
                foreach (var userBook in usersBookList)
                {
                    _userBookService.Delete(userBook);
                }
            }

            return new SuccessResult(Messages.AllUserBookDeletedSuccessfully);
        }

        private IResult DeleteUserFromUserOperationClaims(int userId)
        {
            var result = _userOperationClaimService.DeleteForUsersOwnClaim(userId);
            return result;
        }

        private bool UserCurrentPasswordChecker(string currentPassword, int userId, out User existUser)
        {
            existUser = GetById(userId).Data;
            var existUsersCurrentPasswordChecker = HashingHelper.VerifyPasswordHash(currentPassword, existUser.PasswordHash, existUser.PasswordSalt);

            return existUsersCurrentPasswordChecker;
        }

        private List<UserWithDetailsAndRolesDto> UserToUserWithDetailsAndRolesDto(int userId = 0)
        {
            List<UserWithDetailsAndRolesDto> userWithDetailsAndRolesDtos = new List<UserWithDetailsAndRolesDto>();
            var users = new List<User>();
            if (userId == 0)
            {
                users = GetAll().Data;
            }
            else
            {
                users.Add(GetById(userId).Data);
            }
            foreach (var userAlias in users)
            {
                UserWithDetailsAndRolesDto dto = new UserWithDetailsAndRolesDto
                {
                    Email = userAlias.Email,
                    FirstName = userAlias.FirstName,
                    LastName = userAlias.LastName,
                    UserId = userAlias.Id,
                    UserRoleNames = new List<string>()
                };

                userWithDetailsAndRolesDtos.Add(dto);
            }

            return userWithDetailsAndRolesDtos;
        }
    }
}
