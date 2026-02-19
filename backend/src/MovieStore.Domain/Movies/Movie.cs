using MovieStore.Domain.Users;

namespace MovieStore.Domain.Movies;

public class Movie(string title, string description, decimal price)
{
    public int Id { get; set; }
    public int PublisherProfileId { get; set; }
    
    public string Title { get; set; } = title;
    public const int TitleMaxLength = 150;
    
    public string Description { get; set; } = description;
    public const int DescriptionMaxLength = 2000;
    
    public decimal Price { get; set; } = price;
    public const decimal PriceMaxValue = 9999.99M;
    public const int PricePrecision = 6;
    public const int PriceScale = 2;
    
    public string PosterKey { get; set; }
    public const int PosterKeyMaxLength = 100;
    
    public DateTime CreatedAt { get; init; }
    
    public PublisherProfile PublisherProfile { get; set; }
    public List<MovieGenre> MovieGenres { get; set; }
    public List<MovieActor> MovieActors { get; set; }
    public List<MoviePrivateAsset> MoviePrivateAssets { get; set; }
    public List<MoviePublicAsset> MoviePublicAssets { get; set; }
}