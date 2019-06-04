using Books.Logic.Dtos;
using FluentValidation;

namespace Books.Api.Validators
{
    public class BookValidator : AbstractValidator<BookRequestDto>
    {
        public BookValidator()
        {
            RuleFor(b => b.Title).NotEmpty();
            RuleFor(b => b.Author).NotEmpty();
        }
    }
}
