using ErrorOr;
using MediatR;
using MovieStore.Application.Common.Interfaces;
using MovieStore.Application.Common.Interfaces.Repositories;

namespace MovieStore.Application.Actors.Commands;

public class CreateActorCommandHandler(
    IActorRepository actorRepository,
    IUnitOfWork unitOfWork,
    IFileService fileService) : IRequestHandler<CreateActorCommand, ErrorOr<Success>>
{
    public Task<ErrorOr<Success>> Handle(CreateActorCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}