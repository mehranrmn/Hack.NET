using Microsoft.EntityFrameworkCore;
using Files.Models;
using Files.Services;
using Metadata.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<FileContext>(options => options.UseInMemoryDatabase("Files"));
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<MetadataService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// This switch allows creating new processes while deserialization.
AppContext.SetSwitch("Switch.System.Runtime.Serialization.SerializationGuard.AllowProcessCreation", true);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
