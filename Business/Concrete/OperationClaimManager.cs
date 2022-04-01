using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
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
        private readonly IUserOperationClaimService _userOperationClaimService;
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

        public OperationClaimManager(IOperationClaimDal operationClaimDal, IUserOperationClaimService userOperationClaimService)
        {
            _operationClaimDal = operationClaimDal;
            _userOperationClaimService = userOperationClaimService;
        }
        [SecuredOperation("admin,user.admin")]
        public IDataResult<List<OperationClaim>> GetAll()
        {
            //Aktif rol yoksa error dön kontrolü yapmadım zira aktif rol yoksa sistem zaten çalışmaz. en az user rolü ekli ve aktif olmalıdır. üstelik bu metodu admin rolüne sahip kullanıcılar kullanabilir :)
            return new SuccessDataResult<List<OperationClaim>>(_operationClaimDal.GetAll(o=>o.Active),
                Messages.GetAllOperationClaimsSuccessfully);
        }
        [SecuredOperation("admin")]
        public IDataResult<List<OperationClaim>> GetPredefinedClaims()
        {
            return new SuccessDataResult<List<OperationClaim>>(_predefinedClaims,
                Messages.PredefinedClaimsListedSuccessfully);
        }
        [SecuredOperation("admin,user.admin")]
        public IDataResult<OperationClaim> GetById(int id)
        {
            var result = _operationClaimDal.Get(c => c.Id == id && c.Active);
            if (result==null)
            {
                return new ErrorDataResult<OperationClaim>(Messages.OperationClaimWrongIdOrClaimNotActive);
            }
            return new SuccessDataResult<OperationClaim>(result, Messages.GetClaimByIdSuccessfully);
        }
        
        public IDataResult<OperationClaim> GetByClaimNameIfClaimActive(string claimName) // apide yok
        {
            var result = _operationClaimDal.Get(o => o.Name == claimName && o.Active);
            if (result==null)
            {
                return new ErrorDataResult<OperationClaim>(Messages.ClaimNotFoundOrNotActive);
            }
            return new SuccessDataResult<OperationClaim>(result, Messages.GetOperationClaimByNameSuccessfully);
        }
        [SecuredOperation("admin")]
        [CacheRemoveAspect("IUserService.Get")]
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
        [CacheRemoveAspect("IUserService.Get")]
        [TransactionScopeAspect]
        public IResult Delete(OperationClaim operationClaim)
        {
            var operationClaimToDelete = GetById(operationClaim.Id);
            if (!operationClaimToDelete.Success)
            {
                return new ErrorResult(operationClaimToDelete.Message);
            }

            var isTheRoleUserOrAdmin = BusinessRules.Run(CanNotDeleteAdminOrUserRole(operationClaimToDelete.Data));
            if (isTheRoleUserOrAdmin!=null)
            {
                return isTheRoleUserOrAdmin;
            }

            operationClaimToDelete.Data.Active = false;
            _operationClaimDal.Update(operationClaimToDelete.Data);

            var deleteClaimFromUsers = DeleteClaimFromAllUsers(operationClaimToDelete.Data);
            if (!deleteClaimFromUsers.Success)
            {
                return new ErrorResult(deleteClaimFromUsers.Message);
            }

            return new SuccessResult(Messages.ClaimDeletedSuccessfully);
        }

        private OperationClaim IsOperationClaimAddedBeforeAndNotActiveNow(OperationClaim operationClaim)
        {
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
            var tryToFindOperationClaim = _operationClaimDal.Get(o => o.Name == operationClaim.Name && o.Active);
            if (tryToFindOperationClaim != null)
            {
                return new ErrorResult(Messages.OperationClaimAlreadyAdded);
            }

            return new SuccessResult();
        }

        private IResult CanNotDeleteAdminOrUserRole(OperationClaim operationClaim)
        {
            const string userRoleName = "user";
            const string adminRoleName = "admin";
            if (operationClaim.Name==userRoleName || operationClaim.Name == adminRoleName)
            {
                return new ErrorResult(Messages.CanNotDeleteUserOrAdminRole);
            }

            return new SuccessResult();
        }

        private IResult DeleteClaimFromAllUsers(OperationClaim operationClaim)
        {
            var findDeletedClaimFromUserOperationClaims = _userOperationClaimService.GetByClaimId(operationClaim.Id);
            if (findDeletedClaimFromUserOperationClaims.Success)
            {
                foreach (var userOperationClaim in findDeletedClaimFromUserOperationClaims.Data)
                {
                    _userOperationClaimService.DeleteClaimFromAllUsersWhenClaimDeleted(userOperationClaim);
                    
                }
                return new SuccessResult(Messages.DeletedRoleDeletedByUserAtTheSameTime);
            }

            return new ErrorResult(findDeletedClaimFromUserOperationClaims.Message);

        }

    }
}
