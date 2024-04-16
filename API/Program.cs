using System.Text;
using API.Data;
using API.Entities;
using API.Repository;
using API.Repository.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        Description = "Bearer + your token",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<UrlContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityCore<User>(options => { options.User.RequireUniqueEmail = true; })
    .AddRoles<Role>()
    .AddEntityFrameworkStores<UrlContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateSignatureLast = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UrlShorteningService>();
builder.Services.AddScoped<IShortenRepository, ShortenRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.ConfigObject.AdditionalItems.Add("persistAuthorization", "true"); });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(options =>
{
    options.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:3000");
});

app.UseHttpsRedirection();

// var scope = app.Services.CreateScope();
// var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
// var context = scope.ServiceProvider.GetRequiredService<UrlContext>();
//
// try
// {
//     context.Database.Migrate();
//     await DbInitializer.Initialize(context, userManager);
// }
// catch (Exception e)
// {
//     Console.WriteLine(e);
//     throw;
// }

app.MapControllers();
app.Run();