using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Core.Utilities.StringEditor;
using Entities.DTOs;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private IOperationClaimService _operationClaimService;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IOperationClaimService operationClaimService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _operationClaimService = operationClaimService;
        }

        [ValidationAspect(typeof(UserForRegisterValidator))]
        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = StringEditorHelper.TrimStartAndFinish(userForRegisterDto.Email),
                FirstName = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(userForRegisterDto.FirstName)),
                LastName = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(userForRegisterDto.LastName)),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            if (_userService.Add(user).Success)
            {
                return new SuccessDataResult<User>(user, Messages.UserRegistered);
            }

            return new ErrorDataResult<User>(_userService.Add(user).Message);

        }
        [ValidationAspect(typeof(UserForLoginValidator))]
        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(StringEditorHelper.TrimStartAndFinish(userForLoginDto.Email)).Data;
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }
            return new SuccessDataResult<User>(userToCheck, Messages.LoginSuccessfull);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email).Data != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            //rol yoksa rolsüz token oluşur. Token her türlü oluşur içinde rol olmaz. Ancak rolsüz sistem kullanılamayacağı için error kontrolü yaptım. Rol yoksa token da yok.
            var claims = BeforeGettingAllRolesCheckUserHasAnyRoleAndCheckUserHasUserRoleInOwnRoleList(user);
            if (!claims.Success)
            {
                return new ErrorDataResult<AccessToken>(claims.Message);
            }
            var accessToken = _tokenHelper.CreateToken(user, claims.Data);
            return new SuccessDataResult<AccessToken>(accessToken,Messages.AccessTokenCreated);
        }

        private IDataResult<List<OperationClaim>> BeforeGettingAllRolesCheckUserHasAnyRoleAndCheckUserHasUserRoleInOwnRoleList(User user)
        {
            var claims = _userService.GetClaims(user);
            if (!claims.Success)
            {
                return new ErrorDataResult<List<OperationClaim>>(Messages.UserHasNoActiveRoleToCreateAccessToken);
            }

            const string userRoleName = "user";
            var userHasUserRole = false;
            foreach (var operationClaim in claims.Data)
            {
                if (operationClaim.Name==userRoleName)
                {
                    userHasUserRole = true;
                    break;
                }
            }

            if (!userHasUserRole)
            {
                return new ErrorDataResult<List<OperationClaim>>(Messages.CanNotReachedUserRoleToCheckRoleBeforeCreateAccessToken);
            }

            return claims;
        }
    }
}
