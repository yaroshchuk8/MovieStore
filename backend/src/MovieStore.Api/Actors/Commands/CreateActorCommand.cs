using MovieStore.Application.Common.DTOs;

namespace MovieStore.Api.Actors.Commands;

public record CreateActorCommand(string Name, FileDescriptor? Image);