using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvc.Data;
using mvc.Models;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services au conteneur.
builder.Services.AddControllersWithViews();

// Configuration de la version MySQL
var serverVersion = new MySqlServerVersion(new Version(8, 0, 33)); // Vérifie ta version MySQL locale.
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion)
);

// Configuration d'Identity avec les options de sécurité
builder.Services.AddIdentity<Teacher, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configurez le pipeline HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Active les fichiers statiques (dossier wwwroot).

app.UseRouting();

app.UseAuthentication(); // Ajout de l'authentification.
app.UseAuthorization();

// Ajoutez les rôles au démarrage (si nécessaire)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Teacher>>();

    string[] roleNames = { "Teacher", "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var user = await userManager.FindByEmailAsync("teacher@domain.com");
    if (user == null)
    {
        var teacherUser = new Teacher { UserName = "teacher@domain.com", Email = "teacher@domain.com" };
        await userManager.CreateAsync(teacherUser, "Password123!");
        await userManager.AddToRoleAsync(teacherUser, "Teacher");
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
