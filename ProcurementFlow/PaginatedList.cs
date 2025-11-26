using Microsoft.EntityFrameworkCore;

namespace ProcurementFlow
{
    public class PaginatedList<T>: List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }
        public bool HasPreviousPage => PageIndex > 1; // used by the previous button to check pages
        public bool HasNextPage => PageIndex < TotalPages; // used by the next button to check pages
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int count, int pageIndex, int pageSize)
        {

            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(); // this right selects the items to show in grid
            // .skip() skips that many items from the entire list
            // pageIndex gives the page number which is displayed now, so for 1st page (1-1)*50 gives 0, means for 1st page skip 0items and so on
            // .take() takes up only that amount of items from the list
            return new PaginatedList<T>(items, count, pageIndex, pageSize); // returns the object so that attributes like haspreviouspage can be used
        }

        public static PaginatedList<T> CreateFromList(List<T> source, int pageIndex, int pageSize)
        {
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, source.Count, pageIndex, pageSize);
        }
    }
}