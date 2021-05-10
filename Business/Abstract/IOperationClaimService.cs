using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Core.Utilities.Results;

namespace Business.Abstract
{
    public interface IOperationClaimService
    {
        IDataResult<List<OperationClaim>> GetAll();
        IDataResult<List<OperationClaim>> GetPredefinedClaims();
        IDataResult<OperationClaim> GetById(int id);
        IDataResult<OperationClaim> GetByClaimName(string claimName); //user manager kullanıyor.
        IResult Add(OperationClaim operationClaim);
        IResult Delete(OperationClaim operationClaim);
        
    }
}
