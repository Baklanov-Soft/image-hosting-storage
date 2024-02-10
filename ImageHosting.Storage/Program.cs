using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using ImageHosting.Persistence.DbContexts;
using ImageHosting.Persistence.Extensions.DependencyInjection;
using ImageHosting.Storage.Extensions.DependencyInjection;
using ImageHosting.Storage.Features.Images.Extensions;
using ImageHosting.Storage.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMinio();
builder.Services.AddImageServices();
builder.Services.AddImageHostingDbContext("ImageHosting");
ProblemDetailsExtensions.AddProblemDetails(builder.Services)
    .AddProblemDetailsConventions();
builder.Services.AddKafkaOptions();
builder.Services.AddInitializeUserBucket();

var app = builder.Build();

app.UseProblemDetails();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

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
