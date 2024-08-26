using LWCCWebsite.Core.Interfaces;
using LWCCWebsite.Core.Services;
using LWCCWebsite.Helpers; // Add this namespace
using LWCCWebsite.Interfaces;
using LWCCWebsite.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.DependencyInjection;
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
builder.Services.Configure<LWCCWebsite.Helpers.SmtpSettings>(builder.Configuration.GetSection("Smtp"));

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register EmailService
builder.Services.AddSingleton<IEmailService, EmailService>();

//add the sass compiler
// builder.Services.AddSingleton<SaasCompiler>();

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

//add Payment Services
builder.Services.AddHttpClient<IPaymentApiService, PaymentApiService>(client =>
{
    client.BaseAddress = new Uri("https://secure.networkmerchants.com/api/transact.php");
    client.DefaultRequestHeaders.Add("Authorization", "Basic your_api_key");
});

WebApplication app = builder.Build();

// Serve static files
app.UseStaticFiles();

// Configure middleware
//app.UseWhen(context => !context.Request.Path.StartsWithSegments("/media"), appBuilder => appBuilder.UseStaticFiles());

// Intercept requests to .aspx pages
/*app.Use(async (context, next) =>
{
    if (context.Request.Path.Value.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
    {
        // Redirect to a Razor Page or MVC view
        context.Request.Path = "/path-to-razor-or-mvc-view";
    }
    await next();
});*/

// Enable routing
app.UseRouting();

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