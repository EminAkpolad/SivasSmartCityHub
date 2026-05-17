var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IlanService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Redis adresin (genelde budur)
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
