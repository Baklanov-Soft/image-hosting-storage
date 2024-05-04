using Asp.Versioning;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using ImageHosting.Storage.Application.Services;
using ImageHosting.Storage.Domain.Messages;
using ImageHosting.Storage.Domain.ValueTypes;
using ImageHosting.Storage.Infrastructure.DbContexts;
using ImageHosting.Storage.Infrastructure.Extensions.DependencyInjection;
using ImageHosting.Storage.WebApi.Features.Images.Endpoints;
using ImageHosting.Storage.WebApi.Features.Images.Extensions;
using ImageHosting.Storage.WebApi.OpenApi;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    builder.Services.AddSerilog();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.MapType<UserId>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "uuid"
        });
        options.MapType<ImageId>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "uuid"
        });
    });
    builder.Services.AddMinioServices();
    builder.Services.AddImageServices();
    builder.Services.AddImageHostingDbContext("ImageHosting");
    ProblemDetailsExtensions.AddProblemDetails(builder.Services)
        .AddProblemDetailsConventions();
    builder.Services.AddInitializeUserBucket();
    builder.Services.AddApiVersioning()
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
    builder.Services.ConfigureOptions<NamedSwaggerGenOptions>();
    builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
    builder.Services.AddInfrastructureServices();

    var newImagesTopicName = builder.Configuration["Kafka:NewImagesProducer:TopicName"];
    var bootstrapServers = builder.Configuration.GetSection("Kafka:BootstrapServers").Get<string[]>();

    builder.Services.AddMassTransit(massTransit =>
    {
        massTransit.UsingInMemory();

        massTransit.AddRider(rider =>
        {
            rider.AddProducer<NewImage>(newImagesTopicName);

            rider.UsingKafka((_, kafka) => kafka.Host(bootstrapServers));
        });
    });


    var app = builder.Build();

    app.UseProblemDetails();

    var apiVersionSet = app.NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

    app.MapGroup("api/v{v:apiVersion}")
        .WithApiVersionSet(apiVersionSet)
        .MapImagesEndpoints();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in app.DescribeApiVersions())
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();

                options.SwaggerEndpoint(url, name);
            }
        });
    }

    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<IImageHostingDbContext>();
        dbContext.Migrate();
    }

    using (var serviceScope = app.Services.CreateScope())
    {
        var initializeUserBucket = serviceScope.ServiceProvider.GetRequiredService<IInitializeUserBucket>();
        await initializeUserBucket.CreateDefaultAsync();
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}