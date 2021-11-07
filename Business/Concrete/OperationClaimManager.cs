using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.StringEditor;
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
        public IDataResult<List<OperationClaim>> GetPredefinedClaims()
        {
            return new SuccessDataResult<List<OperationClaim>>(_predefinedClaims,
                Messages.PredefinedClaimsListedSuccessfully);
        }
        [SecuredOperation("admin")]
        public IDataResult<OperationClaim> GetById(int id)
        {
            return new SuccessDataResult<OperationClaim>(_operationClaimDal.Get(c => c.Id == id),
                Messages.GetClaimByIdSuccessfully);
        }
        [SecuredOperation("admin,user")]
        public IDataResult<OperationClaim> GetByClaimName(string claimName)
        {
            return new SuccessDataResult<OperationClaim>(_operationClaimDal.Get(o => o.Name == claimName),
                Messages.GetOperationClaimByNameSuccessfully);
        }
        [SecuredOperation("admin")]
        public IResult Add(OperationClaim operationClaim)
        {
            var isOperationClaimAlreadyExistAndActive =
                BusinessRules.Run(IsOperationClaimAlreadyExistAndActive(operationClaim));

            if (isOperationClaimAlreadyExistAndActive != null)
            {
                return isOperationClaimAlreadyExistAndActive;
            }

            var result = IsOperationClaimAddedBeforeAndNotActiveNow(operationClaim);
            if (result == null)
            {
                /*operaitonClaim.Name = StringEditorHelper.ToTrLocaleCamelCase(StringEditorHelper.ToTrLocaleLowerCase(operationClaim.Name)); */
                operationClaim.Active = true;
                _operationClaimDal.Add(operationClaim);
            }
            else
            {
                _operationClaimDal.Update(result);
            }

            return new SuccessResult(Messages.ClaimAddedSuccessfully);
        }
        [SecuredOperation("admin")]
        public IResult Delete(OperationClaim operationClaim)
        {
            var operationClaimToDelete = GetById(operationClaim.Id).Data;
            operationClaimToDelete.Active = false;
            _operationClaimDal.Update(operationClaimToDelete);
            return new SuccessResult(Messages.ClaimDeletedSuccessfully);
        }

        private OperationClaim IsOperationClaimAddedBeforeAndNotActiveNow(OperationClaim operationClaim)
        {
            /*var nameEditedClaim =
            StringEditorHelper.ToTrLocaleCamelCase(StringEditorHelper.ToTrLocaleLowerCase(operationClaim.Name)); */
            var tryToFindOperationClaim = _operationClaimDal.Get(o => o.Name == operationClaim.Name && o.Active == false);
            if (tryToFindOperationClaim != null)
            {
                tryToFindOperationClaim.Active = true;
                return tryToFindOperationClaim;
            }

            return null;
        }

        private IResult IsOperationClaimAlreadyExistAndActive(OperationClaim operationClaim)
        {
            /*var nameEditedClaim =
            StringEditorHelper.ToTrLocaleCamelCase(StringEditorHelper.ToTrLocaleLowerCase(operationClaim.Name)); */
            var tryToFindOperationClaim = _operationClaimDal.Get(o => o.Name == operationClaim.Name && o.Active);
            if (tryToFindOperationClaim != null)
            {
                return new ErrorResult(Messages.OperationClaimAlreadyAdded);
            }

            return new SuccessResult();
        }

    }
}
