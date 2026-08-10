using ErrorOr;
using MediatR;
using MovieStore.Application.Common.DTOs;

namespace MovieStore.Application.Actors.Commands;

public record CreateActorCommand(string Name, FileDescriptor? Image) : IRequest<ErrorOr<Success>>;