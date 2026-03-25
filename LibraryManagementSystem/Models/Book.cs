namespace LibraryManagementSystem;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public DateTime PublishedDate { get; set; }

    public int AvailableCopies { get; set; }

    public int TotalCopies { get; set; }

    public Book() { }

    public Book(int id, string title, string author, string isbn, DateTime publishedDate, int availableCopies, int totalCopies)
    {
        Id = id;
        Title = title;
        Author = author;
        ISBN = isbn;
        AvailableCopies = availableCopies;
        TotalCopies = totalCopies;
        PublishedDate = publishedDate;
    }
}