using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities;

namespace Repository
{
  public class MPContext : DbContext
  {
    public DbSet<FactSale> Sales { get; set; }
    public DbSet<FactRating> Ratings { get; set; }
    public DbSet<DimMovie> Movies { get; set; }
    public DbSet<DimGenre> Genres { get; set; }
    public DbSet<DimActor> Actors { get; set; }
    public DbSet<DimDirector> Directors { get; set; }

    public MPContext(string connectionString) : base(connectionString)
    {
      if (String.IsNullOrWhiteSpace(connectionString))
        throw new ArgumentNullException("connectionString");

      base.Configuration.LazyLoadingEnabled = false;
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<DimMovie>()
        .HasMany(movie => movie.Genres)
        .WithMany(genre => genre.Movies)
        .Map(map =>
        {
          map.MapLeftKey("MovieId");
          map.MapRightKey("GenreId");
          map.ToTable("GenreMovies");
        });

      modelBuilder.Entity<DimMovie>()
        .HasMany(movie => movie.Actors)
        .WithMany(actor => actor.Movies)
        .Map(map =>
        {
          map.MapLeftKey("MovieId");
          map.MapRightKey("ActorId");
          map.ToTable("ActorMovies");
        });

      modelBuilder.Entity<DimMovie>()
        .HasMany(movie => movie.Directors)
        .WithMany(director => director.Movies)
        .Map(map =>
        {
          map.MapLeftKey("MovieId");
          map.MapRightKey("DirectorId");
          map.ToTable("DirectorMovies");
        });

      base.OnModelCreating(modelBuilder);
    }
    
  }
}
