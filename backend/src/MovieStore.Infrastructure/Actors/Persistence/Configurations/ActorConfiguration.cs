using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Domain.Actors;
using MovieStore.Domain.Common.Constants;
using MovieStore.Infrastructure.Common.Persistence.Constants;

namespace MovieStore.Infrastructure.Actors.Persistence.Configurations;

internal class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Name).HasMaxLength(Actor.NameMaxLength).IsRequired();
        builder.Property(a => a.ImageKey).HasMaxLength(FileConstants.FileKeyMaxLength).IsRequired(false);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql(SqlConstants.UtcDate).IsRequired();

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