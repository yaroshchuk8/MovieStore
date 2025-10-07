using MediatR;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Common.Interfaces.Repositories;
using MovieStore.Domain.Entities;

namespace MovieStore.Application.Genres.Commands.CreateGenre;

public class CreateGenreCommandHandler(IGenreRepository genreRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateGenreCommand, long>
{
    public async Task<long> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = new Genre { Name = request.Name };
        await genreRepository.AddAsync(genre);
        await unitOfWork.CommitChangesAsync();

        return genre.Id;
    }
}