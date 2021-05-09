using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DTOs;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForUpdateValidator:AbstractValidator<UserForUpdateDto>
    {
        public UserForUpdateValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(u => u.FirstName).MinimumLength(3);
            RuleFor(u => u.LastName).NotEmpty();
            RuleFor(u => u.LastName).MinimumLength(2);
            RuleFor(u => u.Email).NotEmpty();
            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.CurrentPassword).NotEmpty();
            RuleFor(u => u.NewPassword).NotEmpty();
        }
    }
}
