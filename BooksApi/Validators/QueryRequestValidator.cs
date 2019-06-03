using BooksApi.Dtos;
using FluentValidation;

namespace BooksApi.Validators
{
    public class QueryRequestValidator : AbstractValidator<QueryRequest>
    {
        public QueryRequestValidator()
        {
            RuleFor(f => f.Take).GreaterThan(0);
            RuleFor(f => f.Skip).GreaterThanOrEqualTo(0);
        }
    }
}