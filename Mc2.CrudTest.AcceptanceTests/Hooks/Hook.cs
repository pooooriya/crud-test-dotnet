using FluentValidation;
using Mc2.CrudTest.Application.Features.Customers.Commands.CreateCustomer;
using Mc2.CrudTest.Application.Mappings;
using Mc2.CrudTest.Domain.Interfaces;
using Mc2.CrudTest.Infrastructure.Persistence;
using Mc2.CrudTest.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;
using BoDi;
using Mc2.CrudTest.Application.Common.Behaviors;
using MediatR;

namespace Mc2.CrudTest.AcceptanceTests.Hooks;

[Binding]
public class Hooks
{
    private static IServiceCollection _services = null!;
    private static IServiceProvider _serviceProvider = null!;
    private readonly IObjectContainer _objectContainer;
    private IServiceScope? _scope;

    public Hooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        _services = new ServiceCollection();
        
        // Add DbContext
        _services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        // Add repositories
        _services.AddScoped<ICustomerRepository, CustomerRepository>();

        // Add MediatR with validation pipeline
        _services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // Add AutoMapper
        _services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Add Validators
        _services.AddValidatorsFromAssemblyContaining(typeof(CreateCustomerCommandValidator));

        _serviceProvider = _services.BuildServiceProvider();
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        _scope = _serviceProvider.CreateScope();
        var dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        
        var mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
        _objectContainer.RegisterInstanceAs(mediator);
    }

    [AfterScenario]
    public void AfterScenario()
    {
        _scope?.Dispose();
    }
}