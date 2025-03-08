using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using TicTacToeMVC.Hubs;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ContentRootPath = @"C:\Users\ee245\Downloads\INTPROG-LAB_ACT2\INTPROG-LAB_ACT2\TicTacToe",
    WebRootPath = @"C:\Users\ee245\Downloads\INTPROG-LAB_ACT2\INTPROG-LAB_ACT2\TicTacToe\wwwroot"
});

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<TicTacToeHub>("/ticTacToeHub");

app.Run();
