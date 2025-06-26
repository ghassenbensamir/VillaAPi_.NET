using MagicVilla_VillaApi;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Logs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

using Serilog;
using MagicVilla_VillaApi.Repsitory.IRepository;
using MagicVilla_VillaApi.Repsitory;
using MagicVilla_VillaApi.Models.VillaNumber;
using MagicVilla_VillaApi.Repsitory.VillaNumberRepo;
using MagicVilla_VillaApi.Repsitory.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using MagicVilla_VillaApi.Models;
using System.Security.Claims;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddControllers(option =>
    {
        option.ReturnHttpNotAcceptable = true;
        option.CacheProfiles.Add("Default30", new CacheProfile()
        {
            Duration = 30
        });
    }
).AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//config Token Auth
builder.Services.AddSwaggerGen(options =>
 {
     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
     {
         Description = "JWT Authorization using the Bearer scheme",
         Name = "Authorization",
         In = ParameterLocation.Header,
         Scheme = "Bearer"
     }
     );
     options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Authorization",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
     options.SwaggerDoc("v1", new OpenApiInfo
     {
         Version = "v1.0",
         Title = "Magic Villa",
         Description = "Api to manage villas",
         TermsOfService = new Uri("https://example.com/terms"),
         Contact = new OpenApiContact
         {
             Name = "Contact",
             Url = new Uri("https://example.com/contact")
         }

     });
     options.SwaggerDoc("v2", new OpenApiInfo
     {
         Version = "v2.0",
         Title = "Magic Villa V2",
         Description = "Api to manage villas v2",
         TermsOfService = new Uri("https://example.com/terms"),
         Contact=new OpenApiContact
         {
             Name = "Contact",
            Url=new Uri("https://example.com/contact")
         }

     });
});
//api Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
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

// Villa Repository
builder.Services.AddScoped<IVillaRepo, VillaRepository>();

//VillaNumber Repository
builder.Services.AddScoped<IVillaNumberRepo, VillaNumberRepo>();

//Users repo
builder.Services.AddScoped<IUserRepo, UserRepo>();

//config authentification
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(x => {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
});

//caching config
builder.Services.AddResponseCaching();

//.NET identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>();


//
var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic Villa V1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic Villa V2");
    });
   
   
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
