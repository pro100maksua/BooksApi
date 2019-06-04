using System;

namespace BooksApi.Exceptions
{
    public class BookNotFoundException : Exception
    {
        private readonly Guid _bookId;

        public override string Message => $"Book with id '{_bookId}' is not found.";

        public BookNotFoundException(Guid bookId)
        {
            _bookId = bookId;
        }
    }
}