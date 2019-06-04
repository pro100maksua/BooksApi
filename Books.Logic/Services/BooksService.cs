using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Books.Data.Entities;
using Books.Data.Interfaces;
using Books.Logic.Dtos;
using Books.Logic.Exceptions;
using Books.Logic.Interfaces;
using Mapster;

namespace Books.Logic.Services
{
    public class BooksService : IBooksService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BooksService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<QueryResult> GetBooksAsync(QueryRequest request)
        {
            var isSearchEmpty = string.IsNullOrWhiteSpace(request.SearchString);
            Expression<Func<Book, bool>> filter = b => isSearchEmpty || b.Title.Contains(request.SearchString);

            var books = await _unitOfWork.BooksRepository.GetAllAsync(request.Skip, request.Take, filter);

            var responseDtos = books.Adapt<IEnumerable<Book>, IEnumerable<BookResponseDto>>();
            var count = await _unitOfWork.BooksRepository.CountAsync();

            return new QueryResult { Books = responseDtos, Count = count };
        }

        public async Task<BookResponseDto> GetAsync(Guid bookId)
        {
            var book = await _unitOfWork.BooksRepository.GetAsync(bookId);
            if (book == null)
            {
                throw new BookNotFoundException(bookId);
            }

            var responseDto = book.Adapt<Book, BookResponseDto>();
            return responseDto;
        }

        public async Task<BookResponseDto> PostAsync(BookRequestDto requestDto)
        {
            var bookExists = await _unitOfWork.BooksRepository.AnyAsync(b => b.Title == requestDto.Title);
            if (bookExists)
            {
                throw new DuplicateBookException(requestDto.Title);
            }

            var book = requestDto.Adapt<BookRequestDto, Book>();

            await _unitOfWork.BooksRepository.AddAsync(book);
            await _unitOfWork.SaveAsync();

            var responseDto = book.Adapt<Book, BookResponseDto>();
            return responseDto;
        }

        public async Task<BookResponseDto> PutAsync(Guid bookId, BookRequestDto requestDto)
        {
            var bookFromDb = await _unitOfWork.BooksRepository.GetAsync(bookId);
            if (bookFromDb == null)
            {
                throw new BookNotFoundException(bookId);
            }

            var titleExists = await _unitOfWork.BooksRepository.AnyAsync(b =>
                b.Title == requestDto.Title && b.Id != bookId);
            if (titleExists)
            {
                throw new DuplicateBookException(requestDto.Title);
            }

            requestDto.Adapt(bookFromDb);
            await _unitOfWork.SaveAsync();

            var responseDto = bookFromDb.Adapt<Book, BookResponseDto>();
            return responseDto;
        }

        public async Task DeleteAsync(Guid bookId)
        {
            var removed = await _unitOfWork.BooksRepository.RemoveAsync(bookId);
            if (!removed)
            {
                throw new BookNotFoundException(bookId);
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
