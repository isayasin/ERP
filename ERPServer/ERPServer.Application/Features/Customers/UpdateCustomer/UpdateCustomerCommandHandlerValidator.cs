using FluentValidation;

namespace ERPServer.Application.Features.Customers.UpdateCustomer;

public sealed class UpdateCustomerCommandHandlerValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandHandlerValidator()
    {
        RuleFor(p => p.TaxNumber).MinimumLength(10).MaximumLength(11);
    }
}
