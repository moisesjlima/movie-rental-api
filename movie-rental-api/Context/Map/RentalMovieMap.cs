using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace movie_rental_api.Context.Map
{
    public class RentalMovieMap : IEntityTypeConfiguration<RentalMovie>
    {
        public void Configure(EntityTypeBuilder<RentalMovie> builder)
        {
            builder.HasKey(x => x.RentalMovieId);
            builder.Property(x => x.ImdbId).IsRequired();
            builder.Property(x => x.CustomerId).IsRequired();
            builder.HasOne(x => x.Customer);
            builder.Property(x => x.RentalStartDate).IsRequired();
            builder.Property(x => x.RentalEndDate).IsRequired();
        }
    }
}