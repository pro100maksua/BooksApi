using System.Collections.Generic;

namespace BooksApi.Dtos
{
    public class QueryResult
    {
        public IEnumerable<BookResponseDto> Books { get; set; }

        public int Count { get; set; }
    }
}