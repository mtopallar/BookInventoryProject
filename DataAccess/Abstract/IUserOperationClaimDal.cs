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
   public interface IUserOperationClaimDal:IEntityRepository<UserOperationClaim>
   {
   }
}
