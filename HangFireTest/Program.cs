using Hangfire;
using Hangfire.Logging;
using HangfireBasicAuthenticationFilter;
using HangFireTest.Hub;
using HangFireTest.Models;
using Microsoft.Extensions.Configuration;
using Serilog;
using TestService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("https://localhost:44384").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
	});
});

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Error()
	.WriteTo.File("logs/Log.txt")
	.CreateLogger();

var hangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection");


builder.Services.AddSignalR();

builder.Services.AddHangfire(x => x.UseSqlServerStorage(hangfireConnectionString));

builder.Services.AddHttpClient();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<CurrencyTrigger>();

builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions()
{
    Authorization = new[]
    {
        new AuthFilter
        {

        }
       
    },
    IgnoreAntiforgeryToken = true
});

app.UseHangfireServer();

app.MapHub<CurrencyHub>("/currency");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

try
{
	app.Run();
}
catch (Exception ex)
{

    throw ex;
}
