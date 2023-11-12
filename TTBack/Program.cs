using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TTBack.Interface;
using TTBack.Models;
using TTBack.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IPasswordHasher<User>, MyPasswordHasher>();
builder.Services.AddTransient<ITripService, TripService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
//var sqlConnection = builder.Configuration["ConnectionString:TTBack:SqlDb"];
var sqlConnection = builder.Configuration.GetConnectionString("TTBack");
builder.Services.AddSqlServer<TrekTogetherContext>(sqlConnection, options => options.EnableRetryOnFailure());
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder
            .WithOrigins("*") // Замените на ваш домен
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
// для запуска используй в консоли dotnet run --urls=http://192.168.178.14:5077

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseCors("MyCorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
