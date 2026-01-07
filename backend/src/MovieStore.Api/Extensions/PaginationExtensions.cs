using System.Text.Json;
using MovieStore.Domain.Common;

namespace MovieStore.Api.Extensions;

public static class PaginationExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, PageInfo metadata)
    {
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata, options));
        response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
    }
}