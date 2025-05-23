﻿using Auth0.AspNetCore.Authentication;
using Quartz;
using Quartz.Impl;
using Radikool6.BackgroundTask;
using Radikool6.Classes;

var confing = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json").Build();
string userHome = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
Global.BaseDir = Path.Combine(userHome,confing["BaseDir"]);

if (!File.Exists(Define.File.DbFile))
{
    Radikool6.Schemas.Upgrade.Execute();
}

string enckey = Environment.GetEnvironmentVariable("RADIKOOL_ENCKEY");
if (!string.IsNullOrEmpty(enckey))
{
    Console.WriteLine($"RADIKOOL_ENCKEY{enckey.Length}");
    Global.EncKey = enckey;
}
else
{
    Console.WriteLine("Please set the environment variable RADIKOOL_ENCKEY");
    var logger = NLog.LogManager.GetCurrentClassLogger();
    logger.Error("Please set the environment variable RADIKOOL_ENCKEY");
    Environment.Exit(1);
}

string auth0Domain = Environment.GetEnvironmentVariable("AUTH0_DOMAIN") ?? "";
string auth0ClientId = Environment.GetEnvironmentVariable("AUTH0_CLIENT_ID") ?? "";
Console.WriteLine($"AUTH0_ISSUER:{auth0Domain.Length}");
Console.WriteLine($"AUTH0_CLIENT_ID:{auth0ClientId.Length}");

Globals.Core.Run();

//番組表更新とSQLite VACCUMのためのスケジューラー
var schedulerFactory = new StdSchedulerFactory();
var sch = await schedulerFactory.GetScheduler();
await sch.Start();
var jobDetailRT = JobBuilder.Create<RefreshTimeTable>()
                .WithIdentity("RefreshTimeTable")
                .Build();
var triggerRT = TriggerBuilder.Create()
    .WithIdentity("RefreshTimeTable")
    .StartNow()
    .WithCronSchedule("0 0 6 ? * * *")
    .Build();
await sch.ScheduleJob(jobDetailRT, triggerRT);

var jobDetailVc = JobBuilder.Create<SqliteVacuum>()
                .WithIdentity("SqliteVacuum")
                .Build();
var triggerVc = TriggerBuilder.Create()
    .WithIdentity("SqliteVacuum")
    .StartNow()
    .WithCronSchedule("0 30 6 ? * * *")
    .Build();
await sch.ScheduleJob(jobDetailVc, triggerVc);

var builder = WebApplication.CreateBuilder();
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddMvc();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllersWithViews();
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = auth0Domain;
    options.ClientId = auth0ClientId;
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