using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
   public class GenreValidator:AbstractValidator<Genre>
    {
        public GenreValidator()
        {
            RuleFor(g => g.Name).NotEmpty();
            RuleFor(g => g.Name).MinimumLength(4);
        }
    }
}
