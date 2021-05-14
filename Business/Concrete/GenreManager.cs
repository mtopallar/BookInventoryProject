using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using Core.Utilities.StringEditor;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class GenreManager:IGenreService
    {
        private readonly IGenreDal _genreDal;

        public GenreManager(IGenreDal genreDal)
        {
            _genreDal = genreDal;
        }
        [SecuredOperation("admin,genre.admin,user")]
        [CacheAspect()]
        public IDataResult<List<Genre>> GetAll()
        {
            return new SuccessDataResult<List<Genre>>(_genreDal.GetAll(), Messages.GetAllGenresSuccessfully);
        }
        [SecuredOperation("admin,genre.admin,user")]
        public IDataResult<Genre> GetById(int id)
        {
            return new SuccessDataResult<Genre>(_genreDal.Get(g => g.Id == id), Messages.GetGenreByIdSuccessfully);
        }
        [SecuredOperation("admin,genre.admin")]
        [CacheRemoveAspect("IGenreService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(GenreValidator))]
        public IResult Add(Genre genre)
        {
            genre.Name = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(genre.Name));
            _genreDal.Add(genre);
            return new SuccessResult(Messages.GenreAddedSuccessfully);
        }
        [SecuredOperation("admin,genre.admin")]
        [CacheRemoveAspect("IGenreService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(GenreValidator))]
        public IResult Update(Genre genre)
        {
            genre.Name = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(genre.Name));
            _genreDal.Update(genre);
            return new SuccessResult(Messages.GenreUpdatedSuccessfully);
        }
        [SecuredOperation("admin,genre.admin")]
        [CacheRemoveAspect("IGenreService.Get")]
        [TransactionScopeAspect]
        public IResult Delete(Genre genre)
        {
            _genreDal.Delete(genre);
            return new SuccessResult(Messages.GenreDeletedSuccessfully);
        }
    }
}
