using SpelArkad.Models;
using SpelArkad.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// L�gg till tj�nster f�r MVC (controllers + views)
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<SpelService>(client =>
{
    client.BaseAddress = new Uri("https://informatik7.ei.hv.se/Gameapi/api/Spel");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});


// L�gg till SpelService som en beroende-tj�nst
builder.Services.AddScoped<SpelService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();
app.UseCors("AllowAll");

// Ignorera SSL-fel endast i utvecklingsmilj�
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

// Om applikationen inte �r i utvecklingsl�ge, visa felhantering
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Aktivera HTTPS och routing
app.UseHttpsRedirection();
app.UseRouting();

// **L�gg till st�d f�r statiska filer (CSS, JS, bilder)**
app.UseStaticFiles();

// Anv�nd CORS om det beh�vs
app.UseCors("AllowAllOrigins");

// L�gg till standard MVC-konfiguration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
