using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Api.Common.Constants;
using MovieStore.Api.Common.FileStorage;
using MovieStore.Api.Common.Persistence.Constants;

namespace MovieStore.Api.Actors;

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