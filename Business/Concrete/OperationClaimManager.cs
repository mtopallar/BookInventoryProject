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
        private readonly List<OperationClaim> _predefinedClaims = new()
        {
            new OperationClaim {Name = "admin"},
            new OperationClaim {Name = "book.admin"},
            new OperationClaim {Name = "author.admin"},
            new OperationClaim {Name = "genre.admin"},
            new OperationClaim {Name = "publisher.admin"},
            new OperationClaim {Name = "user.admin"},
            new OperationClaim {Name = "user"}

        };

        public OperationClaimManager(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }
        [SecuredOperation("admin")]
        public IDataResult<List<OperationClaim>> GetAll()
        {
            //Aktif rol yoksa error dön kontrolü yapmadım zira aktif rol yoksa sistem zaten çalışmaz. en az user rolü ekli ve aktif olmalıdır.
            return new SuccessDataResult<List<OperationClaim>>(_operationClaimDal.GetAll(o=>o.Active),
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
            var result = _operationClaimDal.Get(c => c.Id == id && c.Active);
            if (result==null)
            {
                return new ErrorDataResult<OperationClaim>(Messages.OperationClaimWrongIdOrClaimNotActive);
            }
            return new SuccessDataResult<OperationClaim>(_operationClaimDal.Get(c => c.Id == id && c.Active),
                Messages.GetClaimByIdSuccessfully);
        }
        //[SecuredOperation("admin,user")] sil ya da user rolü için s ecured olmayan bir metod yaz. ama apide olmayacağı için karşılığı yok zaten silinebilir.
        public IDataResult<OperationClaim> GetByClaimNameIfClaimActive(string claimName)
        {
            var result = _operationClaimDal.Get(o => o.Name == claimName && o.Active);
            if (result==null)
            {
                return new ErrorDataResult<OperationClaim>(Messages.ClaimNotFoundOrNotActive);
            }
            return new SuccessDataResult<OperationClaim>(result, Messages.GetOperationClaimByNameSuccessfully);
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
