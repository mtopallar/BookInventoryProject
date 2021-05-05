using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.DTOs;

namespace Business.Concrete
{
    public class UserManager:IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetUserClaims(user),
                Messages.GetUsersAllClaimsSuccessfully);
        }

        public IDataResult<List<UserWithDetailsAndRolesDto>> GetAllUserDetailsWithRoles()
        {
            return new SuccessDataResult<List<UserWithDetailsAndRolesDto>>(_userDal
                .GetRolesWithUserDetails(),Messages.GetAllUserDetailsWitrRolesSuccessfully);
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
            throw new NotImplementedException();
        }

        public IDataResult<User> GetByMail(string email)
        {
            throw new NotImplementedException();
        }

        public IDataResult<User> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IResult Add(User user)
        {
            throw new NotImplementedException();
        }

        public IResult Update(UserForRegisterAndUpdateDto userForUpdateDto)
        {
            throw new NotImplementedException();
        }

        public IResult DeleteForAdmin(int userId)
        {
            throw new NotImplementedException();
        }

        public IResult DeleteForUser(string currentPassword, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
