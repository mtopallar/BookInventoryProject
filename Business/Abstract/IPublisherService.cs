using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IPublisherService
    {
        IDataResult<List<Publisher>> GetAll();
        IDataResult<Publisher> GetById(int id);
        IResult Add(Publisher publisher);
        IResult Update(Publisher publisher);
        IResult Delete(Publisher publisher);
    }
}
