namespace BooksApi.Dtos
{
    public class QueryRequest
    {
        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 10;

        public string SearchString { get; set; }
    }
}