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
        IDataResult<List<OperationClaim>> GetClaims(User user); //For auth. (aktif claimleri getiriyor)
        IDataResult<List<UserWithDetailsAndRolesDto>> GetAllUserDetailsWithRoles();
        IDataResult<UserWithDetailsAndRolesDto> GetUserDetailsWithRolesByUserId(int userId);//list ten teke çektim
        IDataResult<List<User>> GetAll(); //api ye yazmadım.
        IDataResult<User> GetByMail(string email); //apiye şimdilik dahil etmedim. Manager lar kullanıyor. Register  ve login için kullanılıyor.
        IDataResult<User> GetById(int id);
        IResult Add(User user); //managerlar kullanıyor apiye yazmadım.
        IResult Update(UserForUpdateDto userForUpdateDto);
        IResult DeleteForAdmin(int userId);
        IResult DeleteForUser(UserForDeleteDto userForDeleteDto);

    }
}
