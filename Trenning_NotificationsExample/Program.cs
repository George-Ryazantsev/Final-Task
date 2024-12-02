using Microsoft.EntityFrameworkCore;
using Trenning_NotificationsExample.Models;
using Trenning_NotificationsExample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PassportContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});
//builder.Services.AddScoped<PassportUpdateService>();
builder.Services.AddHostedService<PassportUpdateHostedService>();
//builder.Services.AddScoped<FileComparer>();
builder.Services.AddHttpClient<FileUpdateService>();
 static IHostBuilder CreateHostBuilder(string[] args) =>

       Host.CreateDefaultBuilder(args)
           .ConfigureAppConfiguration((context, config) =>
           {
               config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
           })
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<Program>();
           });


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

app.Run();
