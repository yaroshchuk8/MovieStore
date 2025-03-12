using Microsoft.EntityFrameworkCore;
using MovieStore.Application.Actors.DTOs;
using MovieStore.Application.Actors.Interfaces;
using MovieStore.Domain.Entities;

namespace MovieStore.Infrastructure.Persistence.Repositories;

public class ActorRepository(MovieStoreDbContext context) : IActorRepository
{
    public async Task CreateAsync(ActorUpsertDto actor)
    {
        // optimised way to add entry, no querying needed
        Actor newActor = new()
        {
            Id = actor.Id, 
            Name = actor.Name,
            ImagePath = actor.ImagePath,
            Movies = actor.MovieIds.Select(id => new Movie
            {
                Id = id
            }).ToList()
        };
        
        // needed for EF Core to update MovieGenre join table
        foreach (var movie in newActor.Movies)
        {
            context.Entry(movie).State = EntityState.Unchanged;
        }
        
        context.Actors.Add(newActor);
        await context.SaveChangesAsync();
    }
}