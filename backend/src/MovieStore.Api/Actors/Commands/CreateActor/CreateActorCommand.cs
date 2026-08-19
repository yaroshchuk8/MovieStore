using MovieStore.Api.Common.DTOs;

namespace MovieStore.Api.Actors.Commands.CreateActor;

public record CreateActorCommand(string Name, FileDescriptor? Image);