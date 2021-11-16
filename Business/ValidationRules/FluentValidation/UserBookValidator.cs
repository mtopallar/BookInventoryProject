using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserBookValidator:AbstractValidator<UserBook>
    {
        public UserBookValidator()
        {
            RuleFor(u => u.UserId).NotEmpty();
            RuleFor(u => u.BookId).NotEmpty();
        }
    }
}
