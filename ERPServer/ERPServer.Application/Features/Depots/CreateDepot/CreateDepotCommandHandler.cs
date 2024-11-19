using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Depots.CreateDepot;

internal sealed class CreateDepotCommandHandler(IDepotRepository depotRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateDepotCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateDepotCommand request, CancellationToken cancellationToken)
    {
        bool isDepotNameExist = await depotRepository.AnyAsync(p => p.Name == request.Name);
        if (isDepotNameExist)
        {
            return Result<string>.Failure("Malesef bu isim kullanılmaktadır");
        }

        Depot depot = mapper.Map<Depot>(request);

        /*Depot depot = new Depot();
        mapper.Map<Depot>(depot);*/

        await depotRepository.AddAsync(depot, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed(depot.Name + " isimli depo kaydedilmiştir.");
    }
}
