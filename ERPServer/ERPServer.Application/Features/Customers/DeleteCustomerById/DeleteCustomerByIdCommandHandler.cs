using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Customers.DeleteCustomerById;

internal sealed class DeleteCustomerByIdCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCustomerByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteCustomerByIdCommand request, CancellationToken cancellationToken)
    {
        bool hasCustomer = await customerRepository.AnyAsync(p => p.Id == request.Id);
        if (!hasCustomer)
        {
            return Result<string>.Failure("Bu Id sistemlerimizde kayıtlı değil.");
        }

        Customer customer = await customerRepository.GetByExpressionAsync(p => p.Id == request.Id);
        string name = customer.Name;

        customerRepository.Delete(customer);
        //await customerRepository.DeleteByIdAsync(request.Id.ToString());
        await unitOfWork.SaveChangesAsync();

        return Result<string>.Succeed(name + " isimli Müşterinin bilgilerini başarıyla sildik!");

    }
}
