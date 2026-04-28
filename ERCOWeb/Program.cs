using ERCOWeb.Models;
using ERCOWeb.Servicios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IServicioEmail, ServicioEmail>();

builder.Services.AddDbContext<ErcoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ERCOContext"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequiredLength = 8; 
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false; 
    options.Password.RequireUppercase = false; 
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ErcoContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";

    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home");
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


using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    var adminEmail = builder.Configuration["AdminSettings:Email"];
    var adminPass = builder.Configuration["AdminSettings:Password"];

    Console.WriteLine($"--> Verificando Admin: {adminEmail}");

    if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPass))
    {
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var newAdmin = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(newAdmin, adminPass);

            if (result.Succeeded)
            {
                Console.WriteLine($"--> ÉXITO: Admin {adminEmail} creado en la base de datos.");
            }
            else
            {
                Console.WriteLine("--> ERROR al crear admin:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                }
            }
        }
        else
        {
            Console.WriteLine($"--> AVISO: El admin {adminEmail} ya existe.");
        }
    }
    else
    {
        Console.WriteLine("--> ERROR: No se pudieron leer los datos de 'AdminSettings' desde los Secretos de Usuario.");
    }
}

app.Run();