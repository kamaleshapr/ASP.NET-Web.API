using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Business;
using TaskManagement.Data;
using TaskManagement.Data.Repository;
using TaskManagement.Domain.IRepository;

var builder = WebApplication.CreateBuilder(args);

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCustomPolicy", options =>
    {
        options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
#endregion

#region AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
#endregion

#region DI
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();


#endregion


builder.Services.AddSwaggerGen();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

#region Db
builder.Services.AddDbContext<TaskManageDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
