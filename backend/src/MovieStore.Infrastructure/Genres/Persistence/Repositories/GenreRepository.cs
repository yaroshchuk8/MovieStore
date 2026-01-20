using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Domain.Genres;
using MovieStore.Infrastructure.Common.Persistence;
using MovieStore.Infrastructure.Common.Persistence.Repositories;

namespace MovieStore.Infrastructure.Genres.Persistence.Repositories;

internal class GenreRepository(MovieStoreDbContext context) : BaseRepository<Genre>(context), IGenreRepository
{
    
}