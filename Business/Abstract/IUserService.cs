﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using Core.Utilities.Results;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<List<OperationClaim>> GetClaims(User user); //For auth.
        IDataResult<List<User>> GetAll();
        IDataResult<User> GetByMail(string email);
        IResult Add(User user);
        IResult Update(User user);
        IResult Delete(User user);

    }
}
