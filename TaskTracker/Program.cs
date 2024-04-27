using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskTracker.BL.Implementations;
using TaskTracker.BL.Interfaces;
using TaskTracker.BL.Models;
using TaskTracker.DL;
using TaskTracker.DL.Implementations;
using TaskTracker.DL.Interfaces;
using TaskTracker.DL.Models;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TaskDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITaskTrackerService, TaskTrackerService>();


builder.Services.AddScoped<ITaskTrackerRepository, TaskTrackerRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



