namespace LibraryManagementSystem;

public class BookManagementService
{
    private readonly List<Book> _books;

    public BookManagementService()
    {
        _books = new List<Book>();
    }

    public List<Book> GetAllBooks()
    {
        return _books;
    }

    public Book GetBookById(int id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }


}