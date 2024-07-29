using BookReview.Core.Entities;
using BookReview.Core.Interfaces;

namespace BookReview.Application.Services
{
    public class BookService
    {
        private readonly IRepository<Book> _bookRepo;

        public BookService(IRepository<Book> bookRepo) 
        {
            _bookRepo = bookRepo;
        }

        public async Task<List<Book>> GetBooks()
        {
            var books = await _bookRepo.GetAllAsync();
            return books.ToList();
        }

        public async Task AddBook(string title)
        {
            var book = new Book { Title = title };
            await _bookRepo.AddAsync(book);
        }
    }
}
