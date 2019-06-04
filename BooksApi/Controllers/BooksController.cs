using System;
using System.Threading.Tasks;
using Books.Logic.Dtos;
using Books.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
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
            return await _booksService.GetBooksAsync(request);
        }

        [HttpGet("{bookId}")]
        public async Task<ActionResult<BookResponseDto>> GetAsync([FromRoute] Guid bookId)
        {
            return await _booksService.GetAsync(bookId);
        }

        [HttpPost]
        public async Task<ActionResult<BookResponseDto>> PostAsync([FromBody] BookRequestDto requestDto)
        {
            return await _booksService.PostAsync(requestDto);
        }

        [HttpPut("{bookId}")]
        public async Task<ActionResult<BookResponseDto>> PutAsync([FromRoute] Guid bookId, [FromBody] BookRequestDto requestDto)
        {
            return await _booksService.PutAsync(bookId, requestDto);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid bookId)
        {
            await _booksService.DeleteAsync(bookId);

            return Ok();
        }
    }
}
