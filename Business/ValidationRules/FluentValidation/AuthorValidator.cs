using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class AuthorValidator:AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            RuleFor(a => a.FirstName).NotEmpty();
            RuleFor(a => a.FirstName).MaximumLength(3);
            RuleFor(a => a.LastName).NotEmpty();
            RuleFor(a => a.LastName).MinimumLength(2);
            RuleFor(a => a.NationalityId).NotEmpty();
        }
    }
}
