using GLMS.API.Middleware;
using GLMS.API.Repositories;
using GLMS.API.Services;
using GLMS_Project.Services;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IContractService, ContractService>();

builder.Services.AddScoped<GLMS_Project.Factories.IContractFactory, GLMS_Project.Factories.ContractFactoryResolver>();

builder.Services.AddScoped<GLMS.API.Services.ICurrencyStrategy, GLMS.API.Services.LiveExchangeRateStrategy>();
builder.Services.AddScoped<GLMS.API.Services.CurrencyConversionService>();

builder.Services.AddScoped<ServiceRequestService>();
builder.Services.AddScoped<LocalFileService>();

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "GLMS API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your-token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {{
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
                { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        },
        Array.Empty<string>()
    }});
});

builder.Services.AddCors(opts => opts.AddPolicy("FrontendPolicy",
    p => p.WithOrigins("http://localhost:5000", "http://glms-frontend-web")
          .AllowAnyMethod()
          .AllowAnyHeader()));

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GLMS API v1"));
}

app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();

public partial class Program { }