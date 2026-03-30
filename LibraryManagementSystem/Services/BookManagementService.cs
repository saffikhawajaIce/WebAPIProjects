namespace LibraryManagementSystem;

using System;
using System.Collections.Generic;
using System.Linq;

public class BookManagementService
{
    private readonly List<Book> _books;

    private static readonly Random _random = new Random();

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

    public int GenerateBookId()
    {
        int newId = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
        // This method can be used to generate a new unique ID for a book when adding it to the collection.
        return newId;
    }


    public string GenerateIsbn13()
    {
        // Step 1: Pick a book prefix (978 or 979)
        string prefix = _random.Next(2) == 0 ? "978" : "979";

        // Step 2: Random registration group (0-9, simplified)
        string group = _random.Next(0, 10).ToString();

        // Step 3: Random publisher code (4 digits)
        string publisher = _random.Next(1000, 9999).ToString();

        // Step 4: Random title code (4 digits)
        string title = _random.Next(1000, 9999).ToString();

        // The first 12 digits before the check digit
        string first12 = prefix + group + publisher + title;

        // Step 5: Calculate the check digit
        // ISBN-13 check digit algorithm:
        // Multiply each of the first 12 digits alternately by 1 and 3,
        // sum them all up, then the check digit = (10 - (sum % 10)) % 10
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            int digit = int.Parse(first12[i].ToString());
            int weight = (i % 2 == 0) ? 1 : 3;
            sum += digit * weight;
        }
        int checkDigit = (10 - (sum % 10)) % 10;

        // Format it with hyphens: prefix-group-publisher-title-check
        return $"{prefix}-{group}-{publisher}-{title}-{checkDigit}";
    }

    public string GenerateRandomIsbn()
    {
        var service = new BookManagementService();
        return service.GenerateIsbn13();
    }

}

