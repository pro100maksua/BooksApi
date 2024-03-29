﻿using System;
using System.Threading.Tasks;
using Books.Logic.Dtos;

namespace Books.Logic.Interfaces
{
    public interface IBooksService
    {
        Task<QueryResult> GetBooksAsync(QueryRequest request);

        Task<BookResponseDto> GetAsync(Guid bookId);

        Task<BookResponseDto> PostAsync(BookRequestDto requestDto);

        Task<BookResponseDto> PutAsync(Guid bookId, BookRequestDto requestDto);
        
        Task DeleteAsync(Guid bookId);
    }
}