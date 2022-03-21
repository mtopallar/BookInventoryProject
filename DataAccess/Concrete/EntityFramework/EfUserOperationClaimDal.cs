using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.Concrete.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.DTOs;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserOperationClaimDal : EfEntityRepositoryBase<UserOperationClaim, BookInventoryProjectContext>, IUserOperationClaimDal
    {
        public List<UserOperationClaimDto> GetUserClaimDtosByUserId(int userId)
        {
            using (var context = new BookInventoryProjectContext())
            {
                var result = from operaionClaims in context.OperationClaims
                    join userOperationClaims in context.UserOperationClaims on operaionClaims.Id equals
                        userOperationClaims.OperationClaimId where userOperationClaims.UserId == userId
                    select new UserOperationClaimDto
                    {
                        Id = userOperationClaims.Id,
                        Name = operaionClaims.Name
                    };
                return result.ToList();
            }
        }
    }
}
