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
   public interface IUserOperationClaimService
    {
        
        IDataResult<List<UserOperationClaim>> GetAll();
        IDataResult<List<OperationClaim>> GetByUserId(int userId);
        IDataResult<List<UserWithDetailsAndRolesDto>> GetAllUserDetailsWithRoles();
        IDataResult<List<UserWithDetailsAndRolesDto>> GetUserDetailsWithRolesByUserId(int userId);
        IResult Add(int userId, int operationClaimId);
        IResult Update(int id,int userId, int operationClaimId);
        IResult Delete(int id);
    }
}
