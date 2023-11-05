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
builder.Services.AddDbContext<TrekTogetherContext>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder
            .WithOrigins("*") // �������� �� ��� �����
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
// ��� ������� ��������� � ������� dotnet run --urls=http://192.168.178.14:5077

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors("MyCorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
