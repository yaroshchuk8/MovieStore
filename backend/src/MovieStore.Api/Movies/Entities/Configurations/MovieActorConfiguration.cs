using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieStore.Api.Movies.Entities.Configurations;

public class MovieActorConfiguration : IEntityTypeConfiguration<MovieActor>
{
    public void Configure(EntityTypeBuilder<MovieActor> builder)
    {
        builder.HasKey(ma => new { ma.MovieId, ma.ActorId });
        builder.Property(ma => ma.CharacterName).HasMaxLength(MovieActor.CharacterNameMaxLength).IsRequired(false);
    }
}