using BooksApi.Dtos;
using FluentValidation;

namespace BooksApi.Validators
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
