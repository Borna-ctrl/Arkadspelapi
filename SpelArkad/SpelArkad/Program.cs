using SpelArkad.Models;
using SpelArkad.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Lägg till tjänster för MVC (controllers + views)
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


// Lägg till SpelService som en beroende-tjänst
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

// Ignorera SSL-fel endast i utvecklingsmiljö
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

// Om applikationen inte är i utvecklingsläge, visa felhantering
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Aktivera HTTPS och routing
app.UseHttpsRedirection();
app.UseRouting();

// **Lägg till stöd för statiska filer (CSS, JS, bilder)**
app.UseStaticFiles();

// Använd CORS om det behövs
app.UseCors("AllowAllOrigins");

// Lägg till standard MVC-konfiguration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
