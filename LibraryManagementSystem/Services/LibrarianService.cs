//i want this class to manage the library, like adding books, removing books, and keeping track of the books in the library
namespace LibraryManagementSystem;

public class LibrarianService
{
    private readonly BookManagementService _bookManagementService;

    private readonly List<Book> _books;

    public LibrarianService(BookManagementService bookManagementService)
    {
        _bookManagementService = bookManagementService;
        _books = new List<Book>();
    }

    public void AddBook(Book book)
    {
        _books.Add(book);
    }

    public void UpdateBook(Book updatedBook)
    {
        var existingBook = GetBookById(updatedBook.Id);
        if (existingBook != null)
        {
            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.ISBN = updatedBook.ISBN;
            existingBook.PublishedDate = updatedBook.PublishedDate;
        }
    }

    public void DeleteBook(int id)
    {
        var bookToRemove = GetBookById(id);
        if (bookToRemove != null)
        {
            _books.Remove(bookToRemove);
        }
    }

    private Book GetBookById(int id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }
}