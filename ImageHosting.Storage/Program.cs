using System.Text.Json.Serialization;
using Asp.Versioning;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Persistence.Extensions.DependencyInjection;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Extensions.DependencyInjection;
using ImageHosting.Storage.Features.Images.Endpoints;
using ImageHosting.Storage.Features.Images.Extensions;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Generic;
using ImageHosting.Storage.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
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
    options.MapType<SizeParam>(() => new OpenApiSchema
    {
        Type = "integer",
        Format = "int32"
    });
});
builder.Services.AddMinio();
builder.Services.AddImageServices();
builder.Services.AddImageHostingDbContext("ImageHosting");
ProblemDetailsExtensions.AddProblemDetails(builder.Services)
    .AddProblemDetailsConventions();
builder.Services.AddKafkaOptions();
builder.Services.AddInitializeUserBucket();
builder.Services.AddApiVersioning()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });
builder.Services.ConfigureOptions<NamedSwaggerGenOptions>();

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