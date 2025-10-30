using EducaDronAPI.Data;
using EducaDronAPI.Models;
using EducaDronAPI.Localization; // <-- añadir
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri)) return false;
                var host = uri.IdnHost.ToLowerInvariant();

                // dev local
                if (host == "localhost" || host == "127.0.0.1") return true;

                // itch.io
                if (host == "itch.io" || host.EndsWith(".itch.io")) return true;

                // itch.zone (usado por el player HTML: p.ej. html-classic.itch.zone)
                if (host == "itch.zone" || host.EndsWith(".itch.zone")) return true;

                // CDNs habituales de itch (ajusta según veas en Network)
                if (host == "v6p9d9t4.ssl.hwcdn.net" || host == "w3g3a5v6.ssl.hwcdn.net") return true;
                // o, si prefieres, permitir el sufijo:
                // if (host.EndsWith(".ssl.hwcdn.net")) return true;

                return false;
            })
            .AllowAnyHeader()
            .AllowAnyMethod();
            // Añade .AllowCredentials() solo si vas a usar cookies cross-site.
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddErrorDescriber<SpanishIdentityErrorDescriber>() // <-- registrar el describer
    .AddDefaultTokenProviders();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS en Azure
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

// CORS ANTES de auth
app.UseCors(MyAllowSpecificOrigins);

// Identity / Auth (estás usando AddIdentity arriba)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/health", () => Results.Ok("OK"));

app.Run();
