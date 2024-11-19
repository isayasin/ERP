using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Depots.DeleteDepotById;

internal sealed class DeleteDepotByIdCommandHandler(IDepotRepository depotRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteDepotByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteDepotByIdCommand request, CancellationToken cancellationToken)
    {
        bool isDepotExist = await depotRepository.AnyAsync(p => p.Id == request.Id);
        if (!isDepotExist)
        {
            return Result<string>.Failure("Bu Id sistemlerimizde kayıtlı değil.");
        }

        Depot depot = await depotRepository.GetByExpressionAsync(p => p.Id == request.Id);

        // DeleteByExpressionAsync 'i daha deneyemedim olmazsa eski yöntemle yaparım.
        await depotRepository.DeleteByExpressionAsync(p => p.Id == request.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed(depot.Name + " isimli depo kayıtlardan silinmiştir.");
    }
}
