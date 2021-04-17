using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface INationalityService
    {
        IDataResult<List<Nationality>> GetAll();
        IDataResult<Nationality> GetById(int id);
        IResult Add(Nationality nationality);
        IResult Update(Nationality nationality);
        IResult Delete(Nationality nationality);
    }
}
