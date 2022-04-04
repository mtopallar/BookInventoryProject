using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.DTOs;

namespace Business.Abstract
{
   public interface IUserOperationClaimService
    {
        
        IDataResult<List<UserOperationClaim>> GetAll(); //iç metod olarak kullanılacak. SecuredOperation a da grek yok.
        IDataResult<List<UserOperationClaim>> GetByUserId(int userId); //iç metod olarak kullanılacak. SecuredOperation a da grek yok.
        IDataResult<List<UserOperationClaimDto>> GetUserClaimDtosByUserId(int userId);
        IDataResult<List<UserOperationClaim>> GetByClaimId(int operationClaimId); //api de olmayacak. OperationClaim manager Delete içinde kullanacak.
        IResult Add(UserOperationClaimWithAttemptingUserIdDto userOperationClaimWithAttemptingUserIdDto);
        IResult AddUserRoleForUsers(UserOperationClaim userClaim); //apide görünmesi gerekmez.
        IResult Update(UserOperationClaimWithAttemptingUserIdDto userOperationClaimWithAttemptingUserIdDto); //apide var ancak fe de kullanmadım
        IResult Delete(UserOperationClaimWithAttemptingUserIdDto userOperationClaimWithAttemptingUserIdDto);
        IResult DeleteClaimFromAllUsersWhenClaimDeleted(UserOperationClaim userOperationClaim); //apide olmayacak iç metod olacak.
        IResult DeleteForUsersOwnClaim(int userId); //apide olması gerekmez.

        // Gerekirse bir DTO ekleyip rol adını kullanıcı adı ile çekebilirim.
    }
}
