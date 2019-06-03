using System;
using System.Threading.Tasks;
using BooksApi.Dtos;
using BooksApi.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<ActionResult<QueryResult>> GetAllAsync([FromQuery] QueryRequest request)
        {
            var responseDtos = await _booksService.GetBooksAsync(request);

            return responseDtos;
        }

        [HttpGet("{bookId}")]
        public async Task<ActionResult<BookResponseDto>> GetAsync([FromRoute] Guid bookId)
        {
            var responseDto = await _booksService.GetAsync(bookId);
            if (responseDto == null)
            {
                return NotFound(bookId);
            }

            return responseDto;
        }

        [HttpPost]
        public async Task<ActionResult<BookResponseDto>> PostAsync([FromBody] BookRequestDto requestDto)
        {
            var result = await _booksService.PostAsync(requestDto);

            return result;
        }

        [HttpPut("{bookId}")]
        public async Task<ActionResult<BookResponseDto>> PutAsync([FromRoute] Guid bookId, [FromBody] BookRequestDto requestDto)
        {
            var result = await _booksService.PutAsync(bookId, requestDto);
            if (result == null)
            {
                return NotFound(bookId);
            }

            return result;
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid bookId)
        {
            var deleted = await _booksService.DeleteAsync(bookId);
            if (!deleted)
            {
                return NotFound(bookId);
            }

            return Ok();
        }
    }
}
