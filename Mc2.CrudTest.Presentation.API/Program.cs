using Mc2.CrudTest.Application.Features.Customers.Commands.CreateCustomer;
using Mc2.CrudTest.Domain.Interfaces;
using Mc2.CrudTest.Infrastructure.Persistence;
using Mc2.CrudTest.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using Mc2.CrudTest.Application.Features.Customers.Commands.UpdateCustomer;
using System.Reflection;
using Mc2.CrudTest.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("Mc2CrudTestDb"));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly);
});

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssembly(typeof(CreateCustomerCommand).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateCustomerCommandValidator>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapControllers();

app.Run();

