using ERPServer.Domain.Entities;
using FluentValidation;

namespace ERPServer.Application.Features.Depots.CreateDepot;

public sealed class CreateDepotCommandValidator : AbstractValidator<Depot>
{
    public CreateDepotCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
