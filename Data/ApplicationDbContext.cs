using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using mvc.Models;

namespace mvc.Data;

// Cette classe est une classe de contexte de base de données,
// elle permet de définir les tables de la base de données
public class ApplicationDbContext : IdentityDbContext<Teacher>
{
    // DbSet pour les tables
    public DbSet<Student> Students { get; set; }
    public DbSet<Event> Events { get; set; } // Ajout de l'entité Event

    // Constructeur de la classe
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
