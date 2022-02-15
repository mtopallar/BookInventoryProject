using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IAuthorService
    {
        IDataResult<List<Author>> GetAll();
        IDataResult<List<Author>> GetAllRegardlessOfActiveStatue(); //aktif durumuna bakmaksızın tümünü getiriyor. (search area için)
        IDataResult<Author> GetById(int id);
        IResult Add(Author author);
        IResult Update(Author author); //id ye göre
        IResult Delete(Author author);
    }
}
