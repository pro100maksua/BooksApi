using System;

namespace Books.Logic.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException(Guid bookId) : base($"Book with id '{bookId}' was not found.")
        {
        }
    }
}