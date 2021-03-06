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
            //error kontrole aslında gerek yok her kullanıcı en az user rolüne sahip olmak zorunda.(sistem tarafından otomatik atanıyor) ama bu projede rol yoksa token oluşmasını istemiyorum onun için buradaki error kontrolü authmanager da access CreateAccessToken içinde kullanıyor olacağım.
            var usersClaims = _userDal.GetUserClaims(user);
            if (usersClaims.Count == 0)
            {
                return new ErrorDataResult<List<OperationClaim>>(Messages.UserHasNoActiveRole); //mesaj silinebilir. mesajı kullanılmıyor. error dönmesi yeterli.
            }
            return new SuccessDataResult<List<OperationClaim>>(usersClaims, Messages.GetUsersAllClaimsSuccessfully);
        }

        [SecuredOperation("admin,user.admin")]
        [CacheAspect()]
        public IDataResult<List<UserWithDetailsAndRolesDto>> GetAllUserDetailsWithRoles()
        {

            var usersDetailsWithoutRoleList = ConvertUserToUserWithDetailsAndRolesDto();
            var addRolesToDtos = InsertRolesToUserDetailDto(usersDetailsWithoutRoleList);
            return new SuccessDataResult<List<UserWithDetailsAndRolesDto>>(addRolesToDtos, Messages.GetAllUserDetailsWithRolesSuccessfully);
        }

        [SecuredOperation("user")]
        public IDataResult<UserWithDetailsAndRolesDto> GetUserDetailsWithRolesByUserId(int userId)
        {
            //error kontrole gerek yok. user yoksa metod kullanılamaz.
            var userDetailWithoutRoleList = ConvertUserToUserWithDetailsAndRolesDto(userId);
            var addRolesToDto = InsertRolesToUserDetailDto(userDetailWithoutRoleList).Single();
            return new SuccessDataResult<UserWithDetailsAndRolesDto>(addRolesToDto, Messages.GetUserDetailsWithRolesByUserIdSuccessfully);
        }

        [SecuredOperation("admin,user.admin")]
        [CacheAspect()]
        public IDataResult<List<User>> GetAll()
        {
            //error a gerek yok. kullanıcı yoksa metod kullanılamaz.
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
        
        public IDataResult<User> GetById(int id)
        {
            var result = _userDal.Get(u => u.Id == id);
            if (result == null)
            {
                return new ErrorDataResult<User>(Messages.WrongUserId);
            }
            return new SuccessDataResult<User>(result, Messages.GetUserByIdSuccessfully);
        }

        [ValidationAspect(typeof(UserValidator))]
        [CacheRemoveAspect("IUserService.Get")]
        [TransactionScopeAspect]
        public IResult Add(User user)
        {
            //user authmanager'dan geliyor.
            const string userRoleName = "user";
            var checkUserRoleBeforeUserAdded = _operationClaimService.GetByClaimNameIfClaimActive(userRoleName);

            if (checkUserRoleBeforeUserAdded.Success)
            {
                // userAdd olduktan sonra id yi otomatik alıyor. AddUserRoleIfNotExist'a giderken id de gitmiş oluyor.
                _userDal.Add(user);
                AddUserRoleIfNotExist(user, checkUserRoleBeforeUserAdded.Data);
                return new SuccessResult(Messages.UserAddedSuccessfully);
            }

            return new ErrorResult(Messages.UserRoleMustBeAddedAndActive);
        }

        [SecuredOperation("user")]
        [ValidationAspect(typeof(UserForUpdateValidator))]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Update(UserForUpdateDto userForUpdateDto)
        {
            if (!GetById(userForUpdateDto.UserId).Success)
            {
                return new ErrorResult(Messages.WrongUserId);
            }

            User user = null;

            var result = BusinessRules.Run(CheckIfNewMailExists(userForUpdateDto.UserId, StringEditorHelper.TrimStartAndFinish(userForUpdateDto.NewEmail)), UpdateUserWithPasswordSaltAndHash(userForUpdateDto, ref user));

            if (result != null)
            {
                return result;
            }

            _userDal.Update(user);
            return new SuccessResult(Messages.UserUpdatedSuccessfully);
        }


        [SecuredOperation("admin,user.admin")]
        [CacheRemoveAspect("IUserService.Get")]
        [TransactionScopeAspect]
        public IResult DeleteForAdmin(DeleteForAdminWithAttemptingUserIdDto deleteForAdminWithAttemptingUserIdDto)
        {
            
            var result = BusinessRules.Run(HasDeletedUserAdminRoleAndAttemptingUserAdminRole(deleteForAdminWithAttemptingUserIdDto), CheckAnyOtherAdminInSystemBeforeDeleteUser(deleteForAdminWithAttemptingUserIdDto.UserId));
            if (result != null)
            {
                return result;
            }
            var userToDelete = GetById(deleteForAdminWithAttemptingUserIdDto.UserId);
            if (!userToDelete.Success)
            {
                return new ErrorResult(Messages.WrongUserId);
            }
            if (DeleteUserBooks(deleteForAdminWithAttemptingUserIdDto.UserId).Success)
            {
                if (DeleteUserFromUserOperationClaims(deleteForAdminWithAttemptingUserIdDto.UserId).Success)
                {
                    _userDal.Delete(userToDelete.Data);
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
            var result = BusinessRules.Run(CheckAnyOtherAdminInSystemBeforeDeleteUser(userForDeleteDto.UserId));
            if (result != null)
            {
                return result;
            }
            if (!GetById(userForDeleteDto.UserId).Success)
            {
                return new ErrorResult(Messages.WrongUserId);
            }
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

        private void AddUserRoleIfNotExist(User user, OperationClaim operationClaim)
        {
            _userOperationClaimService.AddUserRoleForUsers(new UserOperationClaim { UserId = user.Id, OperationClaimId = operationClaim.Id });
        }

        private IResult UpdateUserWithPasswordSaltAndHash(UserForUpdateDto updateUserDto, ref User user)
        {
            if (UserCurrentPasswordChecker(updateUserDto.CurrentPassword, updateUserDto.UserId, out var existUser))
            {
                HashingHelper.CreatePasswordHash(updateUserDto.NewPassword, out var passwordHash, out var passwordSalt);
                user = new User
                {
                    Id = existUser.Id,
                    FirstName = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleTitleCase(updateUserDto.FirstName)),
                    LastName = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleTitleCase(updateUserDto.LastName)),
                    Email = StringEditorHelper.TrimStartAndFinish(updateUserDto.NewEmail),
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                return new SuccessResult();
            }

            return new ErrorResult(Messages.CurrentUserPasswordError);
        }

        private IResult CheckIfNewMailExists(int userId, string email)
        {
            var checkNewMailExists = GetByMail(email);
            if (checkNewMailExists.Success && checkNewMailExists.Data.Id != userId)
            {
                return new ErrorResult(Messages.NewEmailAlreadyExists);
            }

            return new SuccessResult();
        }

        private IResult DeleteUserBooks(int userId)
        {
            //Kitap yoksa count 0 olduğu için direk success dönüyor. Null hatası söz konusu değil.
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

        private List<UserWithDetailsAndRolesDto> ConvertUserToUserWithDetailsAndRolesDto(int userId = 0)
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

        private List<UserWithDetailsAndRolesDto> InsertRolesToUserDetailDto(List<UserWithDetailsAndRolesDto> dtos)
        {
            foreach (var userWithDetailsAndRolesDto in dtos)
            {
                var usersRoles = GetClaims(new User { Id = userWithDetailsAndRolesDto.UserId });

                foreach (var userOperationClaim in usersRoles.Data)
                {
                    userWithDetailsAndRolesDto.UserRoleNames.Add(userOperationClaim.Name);
                }
            }

            return dtos;
        }

        private IResult CheckAnyOtherAdminInSystemBeforeDeleteUser(int userId)
        {
            var getDeletedUsersRoles = _userOperationClaimService.GetByUserId(userId).Data;
            var getAdminRole = _operationClaimService.GetByClaimNameIfClaimActive("admin").Data;
            var getOtherUsersRoles = _userOperationClaimService.GetAll().Data;

            if (getAdminRole != null)
            {
                if (getDeletedUsersRoles.Any(u => u.OperationClaimId == getAdminRole.Id))
                {

                    if (getOtherUsersRoles.Any(userOperationClaim => userOperationClaim.OperationClaimId == getAdminRole.Id && userOperationClaim.UserId != userId))
                    {
                        return new SuccessResult();
                    }


                    return new ErrorResult(Messages.NoAnyOtherAdminInSystem);
                }
            }

            return new SuccessResult();
        }

        private IResult HasDeletedUserAdminRoleAndAttemptingUserAdminRole(DeleteForAdminWithAttemptingUserIdDto deleteForAdminWithAttemptingUserIdDto)
        {
            var getDeletedUserRoles = _userOperationClaimService.GetUserClaimDtosByUserId(deleteForAdminWithAttemptingUserIdDto.UserId);
            if (getDeletedUserRoles.Data.Any(u => u.Name == "admin"))
            {
                if (_userOperationClaimService.GetUserClaimDtosByUserId(deleteForAdminWithAttemptingUserIdDto.AttemptingUserId).Data.Any(u => u.Name == "admin"))
                {
                    return new SuccessResult();
                }

                return new ErrorResult(Messages.AdminCanDeleteAnotherAdminOnly);
            }

            return new SuccessResult();
        }

    }
}

