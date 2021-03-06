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
    public class EfUserDal : EfEntityRepositoryBase<User, BookInventoryProjectContext>, IUserDal
    {
        //public List<OperationClaim> GetUsersAllActiveClaims(User user)
        //{
        //    using (var context = new BookInventoryProjectContext())
        //    {
        //        var result = from operationClaim in context.OperationClaims
        //                     join userOperationClaim in context.UserOperationClaims
        //                         on operationClaim.Id equals userOperationClaim.OperationClaimId
        //                     where userOperationClaim.UserId == user.Id && operationClaim.Active
        //                     select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name, Active = operationClaim.Active };
        //        return result.ToList();
        //    }
        //}


        // Çalışan orijinal method (Interfacedeki 2. metod)
        public List<OperationClaim> GetUserClaims(User user)
        {
            using (var context = new BookInventoryProjectContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id //&& operationClaim.Active
                    // && operationClaim.Active aslında gereksiz roller userdan hard siliniyor. user ve admin rolleri ise silinemez durumda. kullanıcının tüm rolleri arasında user yoksa claim oluşmayacak şekilde ayarlandı.
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name, Active = operationClaim.Active };
                return result.ToList();
            }
        }

        /*
        public List<UserWithDetailsAndRolesDto> GetRolesWithUserDetails(Expression<Func<UserWithDetailsAndRolesDto, bool>> filter = null)
        {
            using (var context = new BookInventoryProjectContext())
            {

                var result = from user in context.Users
                             join userOperationClaim in context.UserOperationClaims on user.Id equals userOperationClaim.UserId
                             join operaitonClaim in context.OperationClaims on userOperationClaim.OperationClaimId equals
                                 operaitonClaim.Id
                             select new UserWithDetailsAndRolesDto
                             {
                                 UserId = user.Id,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 Email = user.Email
                                 //ClaimName = operaitonClaim.Name
                             };
                return filter == null ? result.ToList() : result.Where(filter).ToList();
            }
        }
        */
    }
}
