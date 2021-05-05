using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;

namespace Business.Concrete
{
    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimDal _operationClaimDal;
        private readonly List<OperationClaim> _predefinedClaims = new List<OperationClaim>
        {
            new OperationClaim {Name = "admin"},
            new OperationClaim {Name = "book.admin"},
            new OperationClaim {Name = "author.admin"},
            new OperationClaim {Name = "genre.admin"},
            new OperationClaim {Name = "publisher.admin"},
            new OperationClaim {Name = "nationality.admin"},
            new OperationClaim {Name = "user"},
            new OperationClaim {Name = "user.admin"}

        };

        public OperationClaimManager(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }
        [SecuredOperation("admin")]
        public IDataResult<List<OperationClaim>> GetAll()
        {
            return new SuccessDataResult<List<OperationClaim>>(_operationClaimDal.GetAll(),
                Messages.GetAllOperationClaimsSuccessfully);
        }
        
        [SecuredOperation("admin")]
        public IResult Add()
        {
            var result = CheckRolesIfAlreadyExistorNot();
            if (result != null)
            {
                foreach (OperationClaim operationClaim in result)
                {
                    _operationClaimDal.Add(operationClaim);

                }
                return new SuccessResult(Messages.AllClaimsAddedSuccessfully);
            }

            return new ErrorResult(Messages.ClaimAlreadyAdded);

        }
        [SecuredOperation("admin")]
        public IResult Delete(OperationClaim operationClaim)
        {
            _operationClaimDal.Delete(operationClaim);
            return new SuccessResult(Messages.OperationClaimDeletedSuccessfully);
        }
        
        private List<OperationClaim> CheckRolesIfAlreadyExistorNot()
        {
            List<OperationClaim> claimsToAdd = new List<OperationClaim>();
            

            foreach (OperationClaim predefinedClaim in _predefinedClaims)
            {
                var existingClaims = _operationClaimDal.Get(o => o.Name == predefinedClaim.Name);
                if (existingClaims == null)
                {
                    claimsToAdd.Add(predefinedClaim);
                }
            }

            return claimsToAdd;
        }

    }
}
