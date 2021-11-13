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
        IDataResult<Genre> GetById(int id); //eğer rol aktifse
        IResult Add(Genre genre);
        IResult Update(Genre genre); //idye göre
        IResult Delete(Genre genre);
    }
}
