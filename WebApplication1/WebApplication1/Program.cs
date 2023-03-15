using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using WebApplication1.Configuartion;
using WebApplication1.DatabaseContext;
using WebApplication1.Filters;
using WebApplication1.Interfaces;
using WebApplication1.JwtAuthorization;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Configuration.AddJsonFile("appsettings.json", false, false);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddScoped<IDatabaseManager, DatabaseManager>();
builder.Services.AddSingleton<IApplicationConfiguration>(builder.Configuration.Get<ApplicationConfiguration>());
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.ParameterFilter<RanksParameterFilter>();
    swagger.ParameterFilter<EmployeeParameterFiler>();
    swagger.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
   $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = AuthenticationConfiguration.ISSUER,
        ValidateAudience = true,
        ValidAudience = AuthenticationConfiguration.AUDIENCE,
        ValidateLifetime = true,
        IssuerSigningKey = AuthenticationConfiguration.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true
    };

});
builder.Services.AddAuthorization();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseMiddleware<JwtTokenAuthorization>();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseHttpLogging();
app.MapControllers();
app.Run();
