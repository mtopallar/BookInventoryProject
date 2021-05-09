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
        IDataResult<OperationClaim> GetByClaimName(string claimName);
        IResult Add(OperationClaim operationClaim);
        IResult Delete(OperationClaim operationClaim);


        //Aşağıdaki operasyonlar gereksiz konuma düştü. Rolü öntanımlı olarak ekliyorum.
        //IDataResult<OperationClaim> GetById(int id);
        //IResult Update(OperationClaim operationClaim);

    }
}
