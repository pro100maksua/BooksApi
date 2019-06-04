using Books.Logic.Dtos;
using FluentValidation;

namespace Books.Api.Validators
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