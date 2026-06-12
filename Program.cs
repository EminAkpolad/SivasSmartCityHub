using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Hesap/Giris";
        options.AccessDeniedPath = "/Hesap/ErisimEngellendi";
    });

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IlanService>();
builder.Services.AddScoped<DuyuruService>();
builder.Services.AddScoped<OneriService>();
builder.Services.AddSingleton<SmartCityHub.Models.MongoDbContext>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; 
    options.InstanceName = "SmartCity_";
});

builder.Services.Configure<SmartCityHub.Settings.MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings")
);
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder
        .Configuration.GetSection("RedisSettings:ConnectionString")
        .Value;
    options.InstanceName = "SmartCity_";
});

builder.Services.AddSingleton<SmartCityHub.Services.MongoDbService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.UseStaticFiles();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
