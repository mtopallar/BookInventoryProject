using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class NationalityValidator:AbstractValidator<Nationality>
    {
        public NationalityValidator()
        {
            RuleFor(n => n.CountryName).NotEmpty();
            RuleFor(n => n.CountryName).MinimumLength(2);
        }
    }
}
