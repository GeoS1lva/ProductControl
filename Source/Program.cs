using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductControl.Application.Interfaces;
using ProductControl.Application.Interfaces.Queries;
using ProductControl.Application.Services;
using ProductControl.Domain.DomainServices;
using ProductControl.Domain.Interfaces;
using ProductControl.Domain.Interfaces.Repositories;
using ProductControl.Domain.Interfaces.Services;
using ProductControl.Infrastructure.Data.Context;
using ProductControl.Infrastructure.Data.Queries;
using ProductControl.Infrastructure.Data.Repositories;
using ProductControl.Infrastructure.Services.Cep;
using ProductControl.Infrastructure.Services.Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PostgreDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
            ClockSkew = TimeSpan.Zero,
            NameClaimType = System.Security.Claims.ClaimTypes.Name,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var blacklistService = context.HttpContext.RequestServices.GetRequiredService<IBlackListRepository>();
                var jti = context.SecurityToken.Id;

                if (await blacklistService.IsTokenRevokedAsync(jti))
                {
                    context.Fail("Este token foi revogado por um logout.");
                }
            }
        };
    });

builder.Services
    .AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build()
    );

builder.Services.AddScoped<IUsersAppService, UsersAppService>();
builder.Services.AddScoped<IProductAppService, ProductAppService>();
builder.Services.AddScoped<IStockMovementAppService, StockMovementAppService>();

builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IUsersQueries, UsersQueries>();
builder.Services.AddScoped<IProductQueries, ProductQueries>();

builder.Services.AddScoped<ICepService, CepService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBlackListRepository, BlackListRepository>();

builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        var secutityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Insira o seu Token abaixo:"
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", secutityScheme);

        document.SecurityRequirements.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }] = Array.Empty<string>()
        });

        return Task.CompletedTask;
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PostgreDbContext>();

    if (context.Database.GetPendingMigrations().Any())
    {
        await context.Database.MigrateAsync();
    }

    await InitializeUser.SeedAdminUser(services);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Product Control API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
