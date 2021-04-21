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
    public class EfUserOperationClaimDal:EfEntityRepositoryBase<UserOperationClaim,BookInventoryProjectContext>,IUserOperationClaimDal
    {
        public List<UserWithDetailsAndRolesDto> GetRolesWithUserDetails(Expression<Func<UserWithDetailsAndRolesDto, bool>> filter=null)
        {
            using (var context = new BookInventoryProjectContext())
            {
                var result = from user in context.Users
                    join userOperationClaim in context.UserOperationClaims on user.Id equals userOperationClaim.UserId
                    join operaitonClaim in context.OperationClaims on userOperationClaim.OperationClaimId equals
                        operaitonClaim.Id
                    select new UserWithDetailsAndRolesDto
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        ClaimName = operaitonClaim.Name
                    };
                return filter == null ? result.ToList() : result.Where(filter).ToList();
            }
        }
    }
}
