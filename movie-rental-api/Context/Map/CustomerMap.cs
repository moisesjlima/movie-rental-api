using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace movie_rental_api.Context.Map
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.CustomerId);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.CPF).HasMaxLength(20);
            builder.Property(x => x.BirthDate).IsRequired();
            builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
            builder.Property(x => x.TelephoneNumber).HasMaxLength(20);
            builder.Property(x => x.CreateDate).IsRequired();
        }
    }
}