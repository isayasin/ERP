using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Products.UpdateProduct;
public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    int TypeValue
    ) : IRequest<Result<string>>;

internal sealed class UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = await productRepository.GetByExpressionAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
        {
            return Result<string>.Failure("Böyle bir ürün kayıtlarda yok!");
        }

        if (product.Name != request.Name)
        {
            bool isProductExist = await productRepository.AnyAsync(p => p.Name == request.Name, cancellationToken);

            if (isProductExist)
            {
                return Result<string>.Failure("Bu isim daha önce kaydedilmiş");
            }
        }

        mapper.Map(request, product);

        productRepository.Update(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed(request.Name + " isimli ürünün bilgileri başarıyla güncellendi");

    }
}
