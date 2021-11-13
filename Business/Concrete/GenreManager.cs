﻿using System;
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
using Core.Utilities.Business;
using Core.Utilities.Results;
using Core.Utilities.StringEditor;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class GenreManager : IGenreService
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
            var result = _genreDal.GetAll(g => g.Active);
            if (result.Count==0)
            {
                return new ErrorDataResult<List<Genre>>(Messages.NoActiveGenreFound);
            }
            return new SuccessDataResult<List<Genre>>(result, Messages.GetAllGenresSuccessfully);
        }
        [SecuredOperation("admin,genre.admin,user")]
        public IDataResult<Genre> GetById(int id)
        {
            var result = _genreDal.Get(g => g.Id == id && g.Active);
            if (result==null)
            {
                return new ErrorDataResult<Genre>(Messages.GenreWrongIdOrClaimNotActive);
            }
            return new SuccessDataResult<Genre>(result, Messages.GetGenreByIdSuccessfully);
        }
        [SecuredOperation("admin,genre.admin")]
        [CacheRemoveAspect("IGenreService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(GenreValidator))]
        public IResult Add(Genre genre)
        {
            var genreExistAndActiveAlready = BusinessRules.Run(IsGenreAlreadyExistAndActive(genre));
            if (genreExistAndActiveAlready != null)
            {
                return genreExistAndActiveAlready;
            }

            var result = IsGenreAddedBeforeAndNotActiveNow(genre);
            if (result == null)
            {
                genre.Name = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(genre.Name));
                genre.Active = true;
                _genreDal.Add(genre);
            }
            else
            {
                _genreDal.Update(result);
            }

            return new SuccessResult(Messages.GenreAddedSuccessfully);
        }
        [SecuredOperation("admin,genre.admin")]
        [CacheRemoveAspect("IGenreService.Get")]
        [TransactionScopeAspect]
        [ValidationAspect(typeof(GenreValidator))]
        public IResult Update(Genre genre)
        {
            var tryToGetGenre = _genreDal.Get(g => g.Id == genre.Id);
            tryToGetGenre.Name = StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(genre.Name));
            _genreDal.Update(tryToGetGenre);
            return new SuccessResult(Messages.GenreUpdatedSuccessfully);
        }
        [SecuredOperation("admin,genre.admin")]
        [CacheRemoveAspect("IGenreService.Get")]
        [TransactionScopeAspect]
        public IResult Delete(Genre genre)
        {
            var genreToDelete = GetById(genre.Id).Data;
            genreToDelete.Active = false;
            _genreDal.Update(genreToDelete);
            return new SuccessResult(Messages.GenreDeletedSuccessfully);
        }

        private Genre IsGenreAddedBeforeAndNotActiveNow(Genre genre)
        {
            var genreNameToFind =
                StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(genre.Name));
            var tryToGetGenre = _genreDal.Get(g => g.Name == genreNameToFind && g.Active == false);
            if (tryToGetGenre != null)
            {
                tryToGetGenre.Active = true;
                return tryToGetGenre;
            }

            return null;
        }

        private IResult IsGenreAlreadyExistAndActive(Genre genre)
        {
            var genreNameToFind =
                StringEditorHelper.TrimStartAndFinish(StringEditorHelper.ToTrLocaleCamelCase(genre.Name));
            var tryToGetGenre = _genreDal.Get(g => g.Name == genreNameToFind && g.Active);

            if (tryToGetGenre != null)
            {
                return new ErrorResult(Messages.GenreAlreadyAdded);
            }

            return new SuccessResult();
        }
    }
}
