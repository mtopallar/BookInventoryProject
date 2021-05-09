using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<List<OperationClaim>> GetClaims(User user); //For auth.
        IDataResult<List<UserWithDetailsAndRolesDto>> GetAllUserDetailsWithRoles();
        IDataResult<List<UserWithDetailsAndRolesDto>> GetUserDetailsWithRolesByUserId(int userId);
        IDataResult<List<User>> GetAll();
        IDataResult<User> GetByMail(string email);
        IDataResult<User> GetById(int id);
        IResult Add(User user);
        IResult Update(UserForUpdateDto userForUpdateDto);
        IResult DeleteForAdmin(int userId);
        IResult DeleteForUser(string currentPassword, int userId);

    }
}
