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
    public interface IBookDal:IEntityRepository<Book>
    {
        List<BookForAddToLibraryDto> GetBooksForAddToLibrary(Expression<Func<BookForAddToLibraryDto, bool>> filter = null);
    }
}
