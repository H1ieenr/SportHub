namespace Shared.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> results { get; set; } = new List<T>();
        public int page_number { get; set; }
        public int page_size { get; set; }
        public int total_count { get; set; }
        public int total_pages => (int)Math.Ceiling(total_count / (double)page_size);
        public bool has_previous_page => page_number > 1;
        public bool has_next_page => page_number < total_pages;

        public PagedResult(IEnumerable<T> results, int count, int pageNumber, int pageSize)
        {
            this.results = results;
            total_count = count;
            page_number = pageNumber;
            page_size = pageSize;
        }

        // Helper để convert nhanh từ Entity sang DTO ngay trong PagedResult
        public PagedResult<TDestination> MapTo<TDestination>(Func<T, TDestination> mapFunc)
        {
            var newResults = results.Select(mapFunc);
            return new PagedResult<TDestination>(newResults, total_count, page_number, page_size);
        }
    }

    public class PaginationParams : BaseRequestDTO
    {
        private const int MaxPageSize = 100;
        public int page_number { get; set; } = 1;
        private int _pageSize = 10;
        public int page_size
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? sort_by { get; set; } = null;
        public string? sort_dir { get; set; } = "asc";
    }
}