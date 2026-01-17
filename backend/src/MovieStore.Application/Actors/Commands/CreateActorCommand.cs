using ErrorOr;
using MediatR;
using MovieStore.Application.Common.Models;

namespace MovieStore.Application.Actors.Commands;

public record CreateActorCommand(string Name, FileDescriptor? Image) : IRequest<ErrorOr<Success>>;