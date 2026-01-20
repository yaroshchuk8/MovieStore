namespace MovieStore.Api.Endpoints;

public static class TestEndpoints
{
    public static void MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/test").WithTags("Test").DisableAntiforgery();

        group.MapPost("file", TestIFormFile);
        group.MapPost("badRequest", TestBadRequest);
    }
    
    private static IResult TestIFormFile(IFormFile file)
    {
        return Results.Ok();
    }
    
    private static IResult TestBadRequest(Test test)
    {
        throw new BadHttpRequestException("Test", StatusCodes.Status400BadRequest);
    }

    private record Test(int Number, string Name);
}