namespace LibraryManagementSystem;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public BooksController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Add your action methods here

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _dbContext.Books.ToListAsync();

        var responseBooks = books.Select(book => new ResponseBookDTO
        {
            Title = book.Title,
            Author = book.Author,
            PublishedDate = book.PublishedDate.ToString("yyyy-MM-dd")
        }).ToList();

        return Ok(responseBooks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _dbContext.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        //return the responsedto instead of the book object
        var responseDto = new ResponseBookDTO
        {
            Title = book.Title,
            Author = book.Author,
            PublishedDate = book.PublishedDate.ToString("yyyy-MM-dd")
        };
        return Ok(responseDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook(CreateBookDTO createBookDto)
    {
        var book = new Book
        {
            Title = createBookDto.Title,
            Author = createBookDto.Author,
            PublishedDate = createBookDto.PublishedDate
        };

        //i want to use the book management service to generate the id for the book instead of using the database generated id
        var bookService = new BookManagementService();
        book.Id = bookService.GenerateBookId();
        book.ISBN = bookService.GenerateIsbn13();

        if (bookService.GetBookById(book.Id) != null)
        {
            return BadRequest("Book with this id already exists");
        }


        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, new ResponseBookDTO
        {
            Title = book.Title,
            Author = book.Author,
            PublishedDate = book.PublishedDate.ToString("yyyy-MM-dd")
        });
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, UpdateBookDTO updateBookDto)
    {
        var book = await _dbContext.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        book.Title = updateBookDto.Title;
        book.Author = updateBookDto.Author;
        book.PublishedDate = updateBookDto.PublishedDate;
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _dbContext.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

}