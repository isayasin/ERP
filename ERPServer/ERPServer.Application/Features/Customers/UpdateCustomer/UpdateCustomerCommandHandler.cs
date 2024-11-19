using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Customers.UpdateCustomer;

internal sealed class UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateCustomerCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        Customer customer = await customerRepository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (customer == null)
        {
            Result<string>.Failure("Müşteri bulunamadı!");
        }

        if (customer!.TaxNumber != request.TaxNumber)
        {
            bool isTaxNumberExists = await customerRepository.AnyAsync(p => p.TaxNumber == request.TaxNumber);
            if (isTaxNumberExists)
            {
                return Result<string>.Failure("Vergi numarası daha önce kaydedilmiş.");
            }
        }

        mapper.Map(request, customer);

        //customerRepository.Update(customer);
        // 22. satırda Tracing'li yaptığımız için yapılan değişiklikler otomatik database'e işliyor.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed(request.Name + " isimli müşterinin bilgileri başarıyla güncellendi!");
    }
}
