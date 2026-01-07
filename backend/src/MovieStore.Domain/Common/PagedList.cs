namespace MovieStore.Domain.Common;

public record PagedList<T>(List<T> Items, PageInfo Metadata)
{
    public static PagedList<T> Create(List<T> items, int pageNumber, int pageSize, int totalCount)
    {
        var meta = new PageInfo(pageNumber, pageSize, totalCount);
        return new PagedList<T>(items, meta);
    }
}