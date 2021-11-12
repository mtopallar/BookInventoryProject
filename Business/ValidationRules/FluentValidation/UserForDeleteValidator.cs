using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Castle.DynamicProxy;
using Core.Entities.Abstract;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Entities.DTOs;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Business.ValidationRules.FluentValidation
{
    public class UserForDeleteValidator:AbstractValidator<UserForDeleteDto>
    {
        public UserForDeleteValidator() {

            RuleFor(u => u.UserId).NotEmpty();
            RuleFor(u => u.UserId).GreaterThan(0);
            RuleFor(u => u.CurrentPassword).NotEmpty();
            RuleFor(u => u.CurrentPassword).NotNull();
        }
    }
}
