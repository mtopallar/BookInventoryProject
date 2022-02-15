using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface IPublisherService
    {
        IDataResult<List<Publisher>> GetAll(); //aktif olanları getir.
        IDataResult<List<Publisher>> GetAllRegardlessOfActiveStatue(); //aktif durumuna bakmaksızın tümünü getiriyor. (search area için)
        IDataResult<Publisher> GetById(int id); //eğer aktifse getir.
        IResult Add(Publisher publisher);
        IResult Update(Publisher publisher); //update olduğu için aktif olmalı haliyle id si de olmalı. id ye göre yaptım.
        IResult Delete(Publisher publisher);
    }
}
