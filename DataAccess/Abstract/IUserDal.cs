using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.Abstract;
using Core.Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Abstract
{
    public interface IUserDal:IEntityRepository<User>
    {
        //public List<OperationClaim> GetUsersAllActiveClaims(User user); bunda claim in aktif olma kontrolü var ancak operationclaim den silinen bir rolü useroperationclaimsden de sildiğim için gerek kalmadı.
        List<OperationClaim> GetUserClaims(User user);
        //List<UserWithDetailsAndRolesDto> GetRolesWithUserDetails(Expression<Func<UserWithDetailsAndRolesDto, bool>> filter=null);
    }
}
