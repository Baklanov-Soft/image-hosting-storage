using ImageHosting.Persistence.Extensions.DependencyInjection;
using ImageHosting.Storage.Extensions.DependencyInjection;
using ImageHosting.Storage.Features.Images;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMinio();
builder.Services.AddImageService();
builder.Services.AddImageMetadataService();
builder.Services.AddImageHostingDbContext("ImageHosting");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
