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
       
    }
}
