using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class OperationClaimValidator:AbstractValidator<OperationClaim> //rolleri ön tanımlı yüklediğim için silinebilir.
    {
        //public OperationClaimValidator()
        //{
        //    RuleFor(o => o.Name).NotEmpty();
        //    RuleFor(o => o.Name).MinimumLength(4);
        //}
    }
}
