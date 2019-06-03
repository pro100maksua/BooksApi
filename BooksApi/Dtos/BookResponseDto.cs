using System;

namespace BooksApi.Dtos
{
    public class BookResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }
    }
}