namespace DarazApp.DTOs
{
    public class PaginationQueryDto
    {
        public string SearchKeyword { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Name";
        public bool Ascending { get; set; } = true;
    }
}
