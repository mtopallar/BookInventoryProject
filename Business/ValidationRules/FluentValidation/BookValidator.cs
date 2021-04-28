using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
    public class BookValidator:AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.Name).MinimumLength(3);
            RuleFor(b => b.PublisherId).NotEmpty();
            RuleFor(b => b.AuthorId).NotEmpty();
            RuleFor(b => b.GenreId).NotEmpty();
            RuleFor(b => b.Isbn).NotEmpty();
            RuleFor(b => b.Isbn).Must(i => i.Length == 13);

        }
    }
}
