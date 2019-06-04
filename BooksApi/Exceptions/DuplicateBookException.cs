using System;

namespace BooksApi.Exceptions
{
    public class DuplicateBookException: Exception
    {
        private readonly string _title;

        public override string Message => $"Book with title '{_title}' already exists.";

        public DuplicateBookException(string title)
        {
            _title = title;
        }
    }
}
