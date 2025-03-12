namespace MovieStore.Application.Actors.DTOs;

public class ActorUpsertDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public List<Guid> MovieIds { get; set; }
}