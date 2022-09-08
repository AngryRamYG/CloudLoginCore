using CloudLogin.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
//builder.Services.AddControllersWithViews()
//    .AddMicrosoftIdentityUI();

//builder.Services.AddAuthorization(options =>
//{
//    // By default, all incoming requests will be authorized according to the default policy
//    //options.FallbackPolicy = options.DefaultPolicy;
//});

builder.Services.AddAuthentication("Cookies").AddCookie(opt =>
{
    opt.Cookie.Name = "HAHAY";
    opt.LoginPath = "/auth/signin";
}
).AddMicrosoftAccount(opt =>
{
    opt.ClientId = builder.Configuration["Microsoft:ClientId"];
    opt.ClientSecret = builder.Configuration["Microsoft:ClientSecret"];
    opt.Events.OnCreatingTicket = context =>
    {
        string picuri = context.User.GetProperty("picture").ToString();
        context.Identity.AddClaim(new Claim("picture", picuri));
        return Task.CompletedTask;
    };
})
.AddGoogle(opt =>
{
    opt.ClientId = builder.Configuration["Google:ClientId"];
    opt.ClientSecret = builder.Configuration["Google:ClientSecret"];
    opt.Scope.Add("openid");
    opt.Events.OnCreatingTicket = context =>
    {
        string picuri = context.User.GetProperty("picture").ToString();
        context.Identity.AddClaim(new Claim("picture", picuri));
        return Task.CompletedTask;
    };
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();
builder.Services.AddSingleton<WeatherForecastService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
