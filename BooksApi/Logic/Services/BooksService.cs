using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BooksApi.Data.Interfaces;
using BooksApi.Dtos;
using BooksApi.Entities;
using BooksApi.Logic.Interfaces;
using Mapster;

namespace BooksApi.Logic.Services
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

            return new QueryResult {Books = responseDtos, Count = count};
        }

        public async Task<BookResponseDto> GetAsync(Guid bookId)
        {
            var book = await _unitOfWork.BooksRepository.GetAsync(bookId);

            var responseDto = book.Adapt<Book, BookResponseDto>();
            return responseDto;
        }

        public async Task<BookResponseDto> PostAsync(BookRequestDto requestDto)
        {
            var book = requestDto.Adapt<BookRequestDto, Book>();

            await _unitOfWork.BooksRepository.AddAsync(book);
            await _unitOfWork.SaveAsync();

            var responseDto = book.Adapt<Book, BookResponseDto>();
            return responseDto;
        }

        public async Task<BookResponseDto> PutAsync(Guid bookId, BookRequestDto requestDto)
        {
            var productFromDb = await _unitOfWork.BooksRepository.GetAsync(bookId);
            if (productFromDb == null)
            {
                return null;
            }

            requestDto.Adapt(productFromDb);
            await _unitOfWork.SaveAsync();
            
            var responseDto = productFromDb.Adapt<Book, BookResponseDto>();
            return responseDto;
        }

        public async Task<bool> DeleteAsync(Guid bookId)
        {
            var removed = await _unitOfWork.BooksRepository.RemoveAsync(bookId);
            if (!removed)
            {
                return false;
            }

            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
