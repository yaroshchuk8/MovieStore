using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Actors;

namespace MovieStore.Infrastructure.Actors.Persistence.Configurations;

internal class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).IsRequired();
        builder.Property(a => a.ImagePath).IsRequired(false);
        builder.Property(a => a.CreatedAt).IsRequired();

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