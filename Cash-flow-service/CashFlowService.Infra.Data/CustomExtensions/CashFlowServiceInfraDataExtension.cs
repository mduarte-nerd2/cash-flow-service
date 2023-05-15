using CashFlowService.Core.OutputPorts;
using CashFlowService.Infra.Data.Contexts;
using CashFlowService.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

public static class CashFlowServiceInfraDataExtension
{
    public static IServiceCollection AddInMemoryDbContext(this IServiceCollection services, Container container)
    {
        services.AddDbContext<InMemoryContext>(options => options.UseInMemoryDatabase(databaseName: "CashServiceMemoryDB"));

        //Registrar o contexto do banco de dados com o SimpleInjector
        container.Register<InMemoryContext>(Lifestyle.Scoped);

        return services;
    }

    public static IServiceCollection AddDbRepository(this IServiceCollection services, Container container)
    {
        services.AddScoped<ICashBookRepository, CashBookRepository>();
        services.AddScoped<ICashBookTransactionRepository, CashBookTransactionRepository>();
        return services;
    }
}
