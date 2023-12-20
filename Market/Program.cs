using Market;
using Market.Logic.Models;
using Market.Logic.Storage;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddDbContext<MarketContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetSection("ConnectionString").Value));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => 
        {
            options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
            options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
        });


builder.Services.AddAuthorization(opts => 
{
    opts.AddPolicy("OnlyForAgents", policy => 
    {
        policy.RequireClaim("role", UserType.Agent.ToString());
    });
    opts.AddPolicy("OnlyForManager", policy => 
    {
        policy.RequireClaim("role", UserType.Manager.ToString());
    });
});

builder.Services.AddMarketServices(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler("/Home/Error");
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
