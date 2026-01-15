namespace MovieStore.Api.Endpoints;

public static class TestEndpoints
{
    public static void MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/test").WithTags("Test").DisableAntiforgery();

        group.MapPost("file", TestIFormFile);
    }
    
    private static IResult TestIFormFile(IFormFile file)
    {
        return Results.Ok();
    }
}