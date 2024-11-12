using ERPServer.Application.Behavior;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ERPServer.Application;
public static class DependencyInjeciton
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjeciton).Assembly);

        services.AddMediatR(conf =>
        {
            conf.RegisterServicesFromAssemblies(typeof(DependencyInjeciton).Assembly);
            conf.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjeciton).Assembly);

        return services;
    }
}
