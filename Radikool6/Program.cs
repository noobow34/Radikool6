using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Radikool6.BackgroundTask;
using Radikool6.Classes;
using Radikool6.Entities;
using System;
using System.IO;

var confing = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json").Build();
Global.BaseDir = confing["BaseDir"];

if (!File.Exists(Define.File.DbFile))
{
    Radikool6.Schemas.Upgrade.Execute();
}

using var con = new SqliteConnection($"Data Source={Define.File.DbFile}");
con.Open();
using SqliteCommand cmd =new ("select key from key", con);
var tempKey =  cmd.ExecuteScalar()?.ToString();
if (!string.IsNullOrEmpty(tempKey))
{
    Global.EncKey = tempKey;
}
else
{
    Global.EncKey = Guid.NewGuid().ToString("N");
    using SqliteCommand cmdKeyInsert = new("insert into key(key) values(@key)",con);
    cmdKeyInsert.Parameters.Add(new SqliteParameter("key", Global.EncKey));
    cmdKeyInsert.ExecuteNonQuery();
}

Globals.Core.Run();

var builder = WebApplication.CreateBuilder();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddMvc();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllersWithViews();
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"] ?? "";
    options.ClientId = builder.Configuration["Auth0:ClientId"] ?? "";
});

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

public static class Globals
{
    public static Core Core = new();
}