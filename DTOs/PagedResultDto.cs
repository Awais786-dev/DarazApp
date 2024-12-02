namespace DarazApp.DTOs
{
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
