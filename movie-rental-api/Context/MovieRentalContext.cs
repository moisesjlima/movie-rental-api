using Microsoft.EntityFrameworkCore;
using movie_rental_api.Context.Map;

namespace movie_rental_api.Context
{
    public class MovieRentalContext : DbContext
    {
        public MovieRentalContext(DbContextOptions<MovieRentalContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<RentalMovie> RentalMovie { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerMap());
            modelBuilder.ApplyConfiguration(new RentalMovieMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}