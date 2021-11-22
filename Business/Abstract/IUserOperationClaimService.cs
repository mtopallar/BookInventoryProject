﻿using System;
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
        
        //IDataResult<List<UserOperationClaim>> GetAll(); //apiden sil. user managerdan dto olarak geliyor.
        //IDataResult<List<UserOperationClaim>> GetByUserId(int userId); //apiden sil. user managerdan dto olarak geliyor.
        IDataResult<List<UserOperationClaim>> GetByClaimId(int operationClaimId); //api de olmayacak. OperationClaim manager Delete içinde kullanacak.
        IResult Add(UserOperationClaim userOperationClaim);
        IResult AddUserRoleForUsers(UserOperationClaim userClaim); //apide görünmesi gerekmez.
        IResult Update(UserOperationClaim userOperationClaim);
        IResult Delete(UserOperationClaim userOperationClaim);
        IResult DeleteForUsersOwnClaim(int userId); //apide olması gerekmez.

        // Gerekirse bir DTO ekleyip rol adını kullanıcı adı ile çekebilirim.
    }
}
