using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using System.Reflection.Metadata;
using Movie.Models.ViewModels;

namespace Movie.Date
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
    {
        public DbSet<Actor> actors { get; set; }
        public DbSet<MovieFilm> movies { get; set; }
        public DbSet<Category> categories { get; set; }

        public DbSet<Cinema> cinemas { get; set; }
        public DbSet<ActorMovie> actorMovies { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MovieE-Ticket;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ActorMovie>().HasKey(t => new { t.ActorId, t.MovieFilmId });
            modelBuilder.Entity<Cart>().HasKey(e=>new {e.ApplicationUserId,e.MovieFilmId});
            modelBuilder.Entity<OrderItem>().HasKey(e => new { e.MovieFilmId, e.OrderId });

        }

    }
}
