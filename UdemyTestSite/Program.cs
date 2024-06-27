using Microsoft.AspNetCore.Mvc.Rendering;
using UdemyTestSite.Interfaces;
using UdemyTestSite.Services;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Services;
using Microsoft.Extensions.Configuration;
using UdemyTestSite.Helpers; // Add this namespace

using Umbraco.Cms.Web.Common.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Umbraco.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewOptions(options =>
    {
        options.HtmlHelperOptions.ClientValidationEnabled = true;
        options.HtmlHelperOptions.Html5DateRenderingMode = Html5DateRenderingMode.Rfc3339;
    });

// Configure Smtp settings
builder.Services.Configure<UdemyTestSite.Helpers.SmtpSettings>(builder.Configuration.GetSection("Smtp"));

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register EmailService
builder.Services.AddSingleton<IEmailService, EmailService>();

// Configure MembershipOptions using the appsettings.json section
var membershipOptions = builder.Configuration.GetSection("Membership").Get<MembershipOptions>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = membershipOptions.PasswordRequiresDigit;
    options.Password.RequireLowercase = membershipOptions.PasswordRequiresLowercase;
    options.Password.RequireUppercase = membershipOptions.PasswordRequiresUppercase;
    options.Password.RequireNonAlphanumeric = membershipOptions.PasswordRequiresNonAlphanumeric;
    options.Password.RequiredLength = membershipOptions.MinimumPasswordLength;
});

// Configure Umbraco services
builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

// Register ApiContentPathProvider
builder.Services.AddScoped<IApiContentPathProvider, ApiContentPathProvider>();

builder.Services.AddScoped<MemberHelper>();

WebApplication app = builder.Build();

//app.UseStaticFiles();

// Configure middleware
//app.UseWhen(context => !context.Request.Path.StartsWithSegments("/media"), appBuilder => appBuilder.UseStaticFiles());

//app.UseRouting();

// Initialize Umbraco
await app.BootUmbracoAsync();

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();


    });

// Configure additional endpoints
app.MapControllerRoute(
    name: "verify",
    pattern: "verify-email",
    defaults: new { controller = "Register", action = "RenderEmailVerification" });
/*app.MapControllerRoute(
    name: "login",
    pattern: "login",
    defaults: new { controller = "Login", action = "RenderLogin" });*/


await app.RunAsync();