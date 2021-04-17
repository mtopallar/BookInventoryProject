using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Abstract
{
    public interface IUserBookDal : IEntityRepository<UserBook>
    {
        List<BookForUserDto> GetBookWithDetails(Expression<Func<BookForUserDto,bool>>filter);
        //en az userId zorunlu o yüzden filter null değil
    }
}
