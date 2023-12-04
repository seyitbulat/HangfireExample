using Hangfire;
using HangfireBasicAuthenticationFilter;
using HangFireTest.Hub;
using HangFireTest.Models;
using TestService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("https://hangfiretest20231129152812.azurewebsites.net").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddSignalR().AddAzureSignalR("Endpoint=https://currency.service.signalr.net;AccessKey=tfT2v6ZPHbw743v0lTTXF6hxW8o9DG5gpenIY2IskDM=;Version=1.0;");

builder.Services.AddHangfire(x => x.UseSqlServerStorage("Data Source =  DESKTOP-R04PVQ3\\SQLEXPRESS; Database = DbPurchasing; Trusted_Connection = true; TrustServerCertificate = true;"));

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

app.Run();
