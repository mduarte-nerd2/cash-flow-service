using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;
using SimpleInjector;
using CashFlowService.Core.DomainEntities;
using CashFlowService.Core.InputPorts;
using CashFlowService.Core.Services;
using CashFlowService.Core.Validators;
using FluentValidation;
using SimpleInjector.Lifestyles;
using AutoMapper;
using CashFlowService.ApiRest.MappingProfiles;

public class Startup
{
    private Container container = new SimpleInjector.Container();
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        container.Options.ResolveUnregisteredConcreteTypes = false;
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvcCore();

        services.AddSimpleInjector(container, options =>
        {
            options.AddAspNetCore()
                .AddControllerActivation();
        });

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerGen();

        services.AddControllers();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
            loggingBuilder.AddConsole();
            loggingBuilder.AddDebug();
        });

        services.AddInMemoryDbContext(container);

        services.AddDbRepository(container);

        RegisterSolutionTypes();

        container.RegisterSingleton(() => GetMapperProfile());
    }

    private void RegisterSolutionTypes()
    {
        container.Register<ICashBookService, CashBookService>(Lifestyle.Scoped);
        container.Register<ICashBookTransactionService, CashBookTransactionService>(Lifestyle.Scoped);
        container.Register<IValidator<CashBook>, CashBookValidator>(Lifestyle.Scoped);
        container.Register<ICashBookManagerFacade, CashBookManagerFacade>(Lifestyle.Scoped);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        app.ApplicationServices.UseSimpleInjector(container);

        app.UseApiVersioning();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
                }

                options.DocExpansion(DocExpansion.List);
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        container.Verify();
    }

    private IMapper GetMapperProfile()
    {
        var mce = new MapperConfigurationExpression();
        mce.ConstructServicesUsing(container.GetInstance);
        mce.AddMaps(typeof(CashBookTransactionProfile).Assembly);
        var mc = new MapperConfiguration(mce);
        mc.AssertConfigurationIsValid();
        IMapper m = new Mapper(mc, t => container.GetInstance(t));
        return m;
    }
}
