using System;
using System.Threading.Tasks;
using Books.Api.Controllers;
using Books.Logic.Dtos;
using Books.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Books.Tests
{
    [TestFixture]
    public class BooksControllerTests
    {
        private Mock<IBooksService> _booksService;
        private BooksController _booksController;

        [SetUp]
        public void Setup()
        {
            _booksService = new Mock<IBooksService>();
            _booksController = new BooksController(_booksService.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnBooks()
        {
            var queryResult = new QueryResult();
            var queryRequest = new QueryRequest();
            _booksService.Setup(bs => bs.GetBooksAsync(queryRequest)).ReturnsAsync(queryResult);

            var result = await _booksController.GetAllAsync(queryRequest);

            Assert.That(result.Value, Is.EqualTo(queryResult));
            _booksService.Verify(bs => bs.GetBooksAsync(queryRequest));
        }
        
        [Test]
        public async Task GetAsync_ReturnBook()
        {
            var bookDto = new BookResponseDto();
            var bookId = Guid.NewGuid();
            _booksService.Setup(bs => bs.GetAsync(bookId)).ReturnsAsync(bookDto);

            var result = await _booksController.GetAsync(bookId);

            Assert.That(result.Value, Is.EqualTo(bookDto));
            _booksService.Verify(bs => bs.GetAsync(It.IsAny<Guid>()));
        }
        
        [Test]
        public async Task PostAsync_ReturnCreatedBook()
        {
            var requestDto= new BookRequestDto();
            var responseDto = new BookResponseDto();
            _booksService.Setup(bs => bs.PostAsync(requestDto)).ReturnsAsync(responseDto);

            var result = await _booksController.PostAsync(requestDto);

            Assert.That(result.Value, Is.EqualTo(responseDto));
            _booksService.Verify(bs => bs.PostAsync(requestDto));
        }
        
        [Test]
        public async Task PutAsync_ReturnUpdatedBook()
        {
            var requestDto = new BookRequestDto();
            var responseDto = new BookResponseDto();
            var bookId = Guid.NewGuid();
            _booksService.Setup(bs => bs.PutAsync(bookId,requestDto)).ReturnsAsync(responseDto);

            var result = await _booksController.PutAsync(bookId, requestDto);

            Assert.That(result.Value, Is.EqualTo(responseDto));
            _booksService.Verify(bs => bs.PutAsync(bookId, requestDto));
        }

        [Test]
        public async Task DeleteAsync_ReturnOk()
        {
            var bookId = Guid.NewGuid();
            _booksService.Setup(bs => bs.DeleteAsync(bookId)).Returns(Task.CompletedTask);

            var result = await _booksController.DeleteAsync(bookId);

            Assert.IsInstanceOf<OkResult>(result);
            _booksService.Verify(bs => bs.DeleteAsync(bookId));
        }
    }
}