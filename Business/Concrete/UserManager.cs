﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
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

        public IDataResult<List<UserWithDetailsAndRolesDto>> GetAllUserDetailsWithRoles()
        {
            return new SuccessDataResult<List<UserWithDetailsAndRolesDto>>(_userDal
                .GetRolesWithUserDetails(), Messages.GetAllUserDetailsWitrRolesSuccessfully);
        }

        public IDataResult<List<UserWithDetailsAndRolesDto>> GetUserDetailsWithRolesByUserId(int userId)
        {
            var getUser = GetById(userId).Data;
            return new SuccessDataResult<List<UserWithDetailsAndRolesDto>>(
                _userDal.GetRolesWithUserDetails(u => u.Email == getUser.Email),
                Messages.GetUserDetailsWithRolesByUserIdSuccessfully);
        }

        public IDataResult<List<User>> GetAll()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetAll(), Messages.GetAllUsersSuccessfully);
        }

        public IDataResult<User> GetByMail(string email)
        {
            return new SuccessDataResult<User>(_userDal.Get(u => u.Email == email),
                Messages.GetUserByEmailSuccessfully);
        }

        public IDataResult<User> GetById(int id)
        {
            return new SuccessDataResult<User>(_userDal.Get(u => u.Id == id), Messages.GetUserByIdSuccessfully);
        }

        public IResult Add(User user)
        {
            _userDal.Add(user);
            AddUserRoleIfNotExist(user);
            return new SuccessResult(Messages.UserAddedSuccessfully);
        }

        public IResult Update(UserForUpdateDto userForUpdateDto)
        {
            User user = null;
            var checkNewMailIsExists = CheckIfNewMailExists(userForUpdateDto.Email);
            var isItOk = UpdateUserWithPasswordSaltAndHash(userForUpdateDto, ref user);
            if (checkNewMailIsExists.Success)
            {
                if (isItOk.Success)
                {
                    _userDal.Update(user);
                    return new SuccessResult(Messages.UserUpdatedSuccessfully);
                }

                return new ErrorResult(isItOk.Message);
            }

            return checkNewMailIsExists;
        }

        public IResult DeleteForAdmin(int userId)
        {
            var userToDelete = GetById(userId).Data;
            if (DeleteUserBooks(userId).Success)
            {
                _userDal.Delete(userToDelete);
            }

            return new SuccessResult(Messages.UserAndUsersBooksDeletedSuccessfullyByAdmin);
        }

        public IResult DeleteForUser(string currentPassword, int userId)
        {
            if (UserCurrentPasswordChecker(currentPassword,userId,out var existUser))
            {
                if (DeleteUserBooks(userId).Success)
                {
                    _userDal.Delete(existUser);
                    return new SuccessResult(Messages.UserAndUsersBooksDeletedSuccessfullyByUser);
                }
            }
            return new ErrorResult(Messages.CurrentUserPasswordError);
        }

        private void AddUserRoleIfNotExist(User user)
        {
            var claimName = "user";
            var isRoleExists = _operationClaimService.GetByClaimName(claimName);
            if (isRoleExists.Success)
            {
                _userOperationClaimService.Add(new UserOperationClaim
                { UserId = user.Id, OperationClaimId = isRoleExists.Data.Id });
            }
            else
            {
                _operationClaimService.Add(new OperationClaim { Name = claimName });
                var addedClaim = _operationClaimService.GetByClaimName(claimName).Data;
                _userOperationClaimService.Add(new UserOperationClaim
                { UserId = user.Id, OperationClaimId = addedClaim.Id });
            }
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
            foreach (var userBook in usersBookList)
            {
                _userBookService.Delete(userBook);
            }

            return new SuccessResult(Messages.AllUserBookDeletedSuccessfully);
        }

        private bool UserCurrentPasswordChecker(string currentPassword, int userId, out User existUser)
        {
            existUser = GetById(userId).Data;
            var existUsersCurrentPasswordChecker = HashingHelper.VerifyPasswordHash(currentPassword, existUser.PasswordHash, existUser.PasswordSalt);

            return existUsersCurrentPasswordChecker;
        }
    }
}
