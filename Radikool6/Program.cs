using Auth0.AspNetCore.Authentication;
using Microsoft.Data.Sqlite;
using Quartz;
using Quartz.Impl;
using Radikool6.BackgroundTask;
using Radikool6.Classes;

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