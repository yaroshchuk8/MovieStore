namespace MovieStore.Application.Actors.DTOs;

public class ActorUpsertDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public List<long> MovieIds { get; set; }
}