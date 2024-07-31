namespace POApplication.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public PagedResponse(IEnumerable<T> data, int pageIndex, int totalPages)
        {
            Data = data;
            PageIndex = pageIndex;
            TotalPages = totalPages;
            Succeeded = true;
        }

        public new IEnumerable<T> Data { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
    }
}
