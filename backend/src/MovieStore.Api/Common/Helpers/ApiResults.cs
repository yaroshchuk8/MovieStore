using ErrorOr;

namespace MovieStore.Api.Helpers;

public static class ApiResults
{
    public static IResult Problem(List<Error> errors)
    {
        if (errors.Count == 0) return Results.Problem();

        return errors.All(error => error.Type == ErrorType.Validation) ? ValidationProblem(errors) : Problem(errors[0]);
    }

    private static IResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Results.Problem(statusCode: statusCode, detail: error.Description);
    }

    private static IResult ValidationProblem(List<Error> errors)
    {
        var modelState = errors.GroupBy(e => e.Code)
            .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());

        return Results.ValidationProblem(modelState);
    }
}