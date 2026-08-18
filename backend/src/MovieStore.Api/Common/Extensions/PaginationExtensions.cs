using System.Text.Json;
using Microsoft.Net.Http.Headers;
using MovieStore.Api.Constants;
using MovieStore.Domain.Common;

namespace MovieStore.Api.Extensions;

public static class PaginationExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, PageInfo metadata)
    {
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        response.Headers.Add(HttpConstants.Headers.XPagination, JsonSerializer.Serialize(metadata, options));
        response.Headers.Add(HeaderNames.AccessControlExposeHeaders, HttpConstants.Headers.XPagination);
    }
}