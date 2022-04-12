using System;
using System.Collections.Generic;
using System.Text;
using Business.Constants;
using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForRegisterValidator : AbstractValidator<UserForRegisterDto>
    {
        public UserForRegisterValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(u => u.FirstName).MinimumLength(3);
            RuleFor(u => u.LastName).NotEmpty();
            RuleFor(u => u.LastName).MinimumLength(2);
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
