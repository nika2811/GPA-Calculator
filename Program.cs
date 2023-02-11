using GPA_Calculator.Db;
using GPA_Calculator.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StudentGradesContext>(
    c => c.UseSqlServer(builder.Configuration["AppDbContextConnection"]));
builder.Services.AddTransient<GpaCalculator>();
builder.Services.AddTransient<GradeCalculator>();
builder.Services.AddTransient<SubjectScores>();
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

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<StudentGradesContext>();
dbContext!.Database.EnsureCreated();

app.Run();