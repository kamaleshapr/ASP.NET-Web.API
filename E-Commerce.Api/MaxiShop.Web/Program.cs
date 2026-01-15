
using MaxiShop.Business.Common;
using MaxiShop.Business.Contracts;
using MaxiShop.Business.Services;
using MaxiShop.Business.Services.Interface;
using MaxiShop.Data.Common;
using MaxiShop.Data.Db;
using MaxiShop.Data.Repository;
using MaxiShop.Data.SeedData;
using MaxiShop.Domain.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(builder =>
{
    builder.AddPolicy("CustomCorsPolicy", options =>
    {
        options.AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin();
    });
});

#region Database Configuration

builder.Services.AddDbContext<ApplicationDbContext>(optionsAction =>
{
    optionsAction.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
{
    // override Identity cookie defaults so JWT is used for incoming requests
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2),
        RoleClaimType = ClaimTypes.Role // ensure role claims are mapped correctly
    };
});

#endregion

#region Services Registration

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<ICatergoryService,CategoryService>();
builder.Services.AddScoped<IBrandService,BrandService>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IAuthService,AuthService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

#endregion

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. " +
        "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
        "\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            // new OpenApiSecurityScheme
            // {
            //    Reference = new OpenApiReference
            //    {
            //        Type = ReferenceType.Header,
            //    },
            //    Scheme = "Oauth2",
            //    Name = "Bearer",
            //    In = ParameterLocation.Header,

            //},
            // AI suggested 
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme, // MUST be SecurityScheme
                    Id = "Bearer"                        // Id matches the AddSecurityDefinition name
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

#region Seed Data
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (db.Database.IsSqlServer())
    {
        db.Database.Migrate();
    }
    await SeedData.SeedDataAsync(db);

}
catch (Exception ex)
{
    var loggerr = app.Services.GetRequiredService<ILogger<Program>>();
    loggerr.LogError(ex, "An error occurred while seeding the database.");
    //throw;
}
await SeedData.SeedRoles(app.Services);
#endregion

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment()) // comment out for swagger to work on deployment in remote server
//{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(SwaggerEndpointOptions =>
    //{
    //    SwaggerEndpointOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    //});
//}

app.UseCors("CustomCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
