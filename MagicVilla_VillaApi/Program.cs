using MagicVilla_VillaApi;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Logs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers(option =>
 option.ReturnHttpNotAcceptable=true).AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//log configuration(write logs to a file using serilog pack)
// Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.
//                     File("Logs/VillaLogs.txt",rollingInterval:RollingInterval.Day).CreateLogger();
// builder.Host.UseSerilog();


// custom logging
// builder.Services.AddSingleton<ILogging, LoggingV2>();


//config MySql Db
var connectionString = builder.Configuration.GetConnectionString("DefaultSQLConnection");
builder.Services.AddDbContext<ApplicationDBContext>(option => option.UseMySql(
                connectionString, ServerVersion.AutoDetect(connectionString)
));


//autoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));
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
