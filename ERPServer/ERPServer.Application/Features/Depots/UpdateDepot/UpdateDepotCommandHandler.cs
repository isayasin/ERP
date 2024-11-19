using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Depots.UpdateDepot;

internal sealed class UpdateDepotCommandHandler(IDepotRepository depotRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateDepotCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateDepotCommand request, CancellationToken cancellationToken)
    {
        Depot depot = await depotRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);
        if (depot == null)
        {
            return Result<string>.Failure("Depo bulunamadı");
        }

        if (depot.Name != request.Name)
        {
            bool isDepotNameExist = await depotRepository.AnyAsync(p => p.Name == request.Name);
            if (isDepotNameExist)
            {
                return Result<string>.Failure("Malesef bu isim kullanılmaktadır.");
            }
        }

        mapper.Map(request, depot);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed(request.Name + " isimli deponun bilgileri başarıyla güncellendi!");
    }
}
