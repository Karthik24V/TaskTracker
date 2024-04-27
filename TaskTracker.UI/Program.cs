using AutoMapper;
using MudBlazor.Services;
using TaskTracker.BL.Models;
using TaskTracker.DL.Models;
using TaskTracker.UI.Components;
using TaskTracker.UI.Models;
using MudBlazor;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddScoped<TaskServices>();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    options.DetailedErrors = true;
});
builder.Services.AddScoped<IBreakpointService, BreakpointService>();
builder.Services.AddHttpClient("TaskTracker", client =>
{
    client.BaseAddress = new Uri("https://localhost:7234/");
    // Additional configuration such as headers, timeouts, etc. can go here
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
