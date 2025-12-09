using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence.Configurations;

internal class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).IsRequired();
        builder.Property(a => a.ImagePath).IsRequired(false);
        builder.Property(a => a.CreateAt).IsRequired();

        // Many-to-Many Relationship
        // builder.HasMany(a => a.Movies)
        //     .WithMany(m => m.Actors)
        //     .UsingEntity(j =>
        //     {
        //         j.ToTable("MovieActors");
        //         j.Property("MoviesId").HasColumnName("MovieId");
        //         j.Property("ActorsId").HasColumnName("ActorId");
        //     });
    }
}