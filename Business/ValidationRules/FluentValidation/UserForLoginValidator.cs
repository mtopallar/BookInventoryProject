using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Constants;
using Entities.DTOs;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForLoginValidator:AbstractValidator<UserForLoginDto>
    {
        public UserForLoginValidator()
        {
            RuleFor(u => u.Email).NotEmpty();
            RuleFor(u => u.Email).EmailAddress();
            RuleFor(u => u.Password).NotEmpty();
            RuleFor(u => u.Password).MinimumLength(5);
            RuleFor(u => u.Password).Matches(@"[A-Z]+").WithMessage(Messages.PasswordNeedsCapitalLetter);
            RuleFor(u => u.Password).Matches(@"[a-z]+").WithMessage(Messages.PasswordNeedsLowerCase);
            RuleFor(u => u.Password).Matches(@"[0-9]+").WithMessage(Messages.PasswordNeedsNumericCharacter);
        }
    }
}
