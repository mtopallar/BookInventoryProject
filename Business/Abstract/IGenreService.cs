using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IGenreService
    {
        IDataResult<List<Genre>> GetAll(); //sadece aktifolanları getiriyor.
        IDataResult<List<Genre>> GetAllRegardlessOfActiveStatue(); //aktif durumuna bakmaksızın tümünü getiriyor. (search area için)
        IDataResult<Genre> GetById(int id); //eğer rol aktifse
        IResult Add(Genre genre);
        IResult Update(Genre genre); //idye göre
        IResult Delete(Genre genre);
    }
}
