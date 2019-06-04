using System;

namespace Books.Logic.Dtos
{
    public class BookResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }
    }
}