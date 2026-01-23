using ErrorOr;
using MediatR;

namespace MovieStore.Application.Users.Commands;

public record CreatePublisherProfileCommand(string StudioName) : IRequest<ErrorOr<Success>>;