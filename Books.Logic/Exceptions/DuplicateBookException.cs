using System;

namespace Books.Logic.Exceptions
{
    public class DuplicateBookException: Exception
    {
        public DuplicateBookException(string title): base($"Book with title '{title}' already exists.")
        {
        }
    }
}
