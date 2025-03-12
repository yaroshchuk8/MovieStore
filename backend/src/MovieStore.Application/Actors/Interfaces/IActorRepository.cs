using MovieStore.Application.Actors.DTOs;
using MovieStore.Domain.Entities;

namespace MovieStore.Application.Actors.Interfaces;

public interface IActorRepository
{
    Task CreateAsync(ActorUpsertDto actor);
}