using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BooksApi.Data.Interfaces;
using BooksApi.Dtos;
using BooksApi.Entities;
using BooksApi.Exceptions;
using BooksApi.Logic.Interfaces;
using BooksApi.Logic.Services;
using Moq;
using NUnit.Framework;

namespace BooksTests
{
    [TestFixture]
    public class BooksServiceTests
    {
        private IBooksService _booksService;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IRepository<Book>> _booksRepository;

        [SetUp]
        public void Setup()
        {
            _booksRepository = new Mock<IRepository<Book>>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _booksService = new BooksService(_unitOfWork.Object);
            _unitOfWork.Setup(uow => uow.BooksRepository).Returns(_booksRepository.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnBooks()
        {
            var request = new QueryRequest();
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Author = "Josh",
                Title = "Nature"
            };
            var books = new[] { book };
            _booksRepository
                .Setup(br => br.GetAllAsync(request.Skip, request.Take, It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync(books);

            var result = await _booksService.GetBooksAsync(request);

            Assert.That(result.Books.Count(), Is.EqualTo(books.Length));
            Assert.True(result.Books.All(b =>
                b.Id == book.Id &&
                b.Title == book.Title &&
                b.Author == book.Author));
            _booksRepository.Verify(pr =>
                pr.GetAllAsync(request.Skip, request.Take, It.IsAny<Expression<Func<Book, bool>>>()));
        }

        [Test]
        public void GetAsync_InvalidId_Throw()
        {
            Assert.ThrowsAsync<BookNotFoundException>(async () => await _booksService.GetAsync(Guid.Empty));

            _booksRepository.Verify(br => br.GetAsync(Guid.Empty));
        }

        [Test]
        public async Task GetAsync_ValidId_ReturnBookDto()
        {
            var bookId = Guid.NewGuid();
            var book = new Book();
            _booksRepository.Setup(br => br.GetAsync(bookId)).ReturnsAsync(book);

            var result = await _booksService.GetAsync(bookId);

            Assert.True(result.Id == book.Id &&
                        result.Title == book.Title &&
                        result.Author == book.Author);
            _booksRepository.Verify(br => br.GetAsync(bookId));
        }

        [Test]
        public void PostAsync_DuplicateTitle_Throw()
        {
            _booksRepository.Setup(br => br.AnyAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync(true);

            Assert.ThrowsAsync<DuplicateBookException>(async () => await _booksService.PostAsync(new BookRequestDto()));
            _booksRepository.Verify(br => br.AnyAsync(It.IsAny<Expression<Func<Book, bool>>>()));
            _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task PostAsync_ValidObject_ReturnNewBook()
        {
            var newBook = new BookRequestDto();
            _booksRepository.Setup(br => br.AnyAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync(false);

            var result = await _booksService.PostAsync(newBook);

            Assert.True(result.Title == newBook.Title && result.Author == newBook.Author);
            _unitOfWork.Verify(uow => uow.SaveAsync());
            _booksRepository.Verify(br => br.AnyAsync(It.IsAny<Expression<Func<Book, bool>>>()));
            _booksRepository.Verify(br => br.AddAsync(It.IsAny<Book>()));
        }

        [Test]
        public void PutAsync_InvalidId_Throw()
        {
            Assert.ThrowsAsync<BookNotFoundException>(async () => await _booksService.PutAsync(Guid.Empty, null));

            _booksRepository.Verify(br => br.GetAsync(Guid.Empty));
            _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Never);
        }

        [Test]
        public void PutAsync_DuplicateTitle_Throw()
        {
            var bookId = Guid.NewGuid();
            var newBook = new BookRequestDto();
            var book = new Book();
            _booksRepository.Setup(br => br.GetAsync(bookId)).ReturnsAsync(book);
            _booksRepository.Setup(br => br.AnyAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync(true);

            Assert.ThrowsAsync<DuplicateBookException>(async () => await _booksService.PutAsync(bookId, newBook));

            _booksRepository.Verify(br => br.GetAsync(bookId));
            _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task PutAsync_ValidObject_ReturnUpdatedProduct()
        {
            var bookId = Guid.NewGuid();
            var newBook = new BookRequestDto
            {
                Author = "Josh",
                Title = "Nature"
            };
            var book = new Book();
            _booksRepository.Setup(br => br.GetAsync(bookId)).ReturnsAsync(book);
            _booksRepository.Setup(br => br.AnyAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync(false);

            var result = await _booksService.PutAsync(bookId, newBook);

            Assert.True(result.Title == newBook.Title && result.Author == newBook.Author);
            _booksRepository.Verify(br => br.GetAsync(bookId));
            _unitOfWork.Verify(uow => uow.SaveAsync());
        }

        [Test]
        public void DeleteAsync_InvalidId_Throw()
        {
            var bookId = Guid.NewGuid();
            _booksRepository.Setup(br => br.RemoveAsync(bookId)).ReturnsAsync(false);

            Assert.ThrowsAsync<BookNotFoundException>(async () => await _booksService.DeleteAsync(bookId));
            _booksRepository.Verify(br => br.RemoveAsync(bookId));
            _unitOfWork.Verify(uow => uow.SaveAsync(), Times.Never);
        }

        [Test]
        public async Task DeleteAsync_ValidId_Delete()
        {
            var bookId = Guid.NewGuid();
            _booksRepository.Setup(br => br.RemoveAsync(bookId)).ReturnsAsync(true);

            await _booksService.DeleteAsync(bookId);

            _booksRepository.Verify(br => br.RemoveAsync(bookId));
            _unitOfWork.Verify(uow => uow.SaveAsync());
        }
    }
}