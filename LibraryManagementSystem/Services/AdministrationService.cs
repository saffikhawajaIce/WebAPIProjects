//i actually dont know what the admin would do here ill be honest
//maybe they can manage the librarians? like adding and removing librarians from the system? or maybe they can manage the library's policies? like setting the borrowing limits or the late fee policies? i guess we can add those features later on if we have time
namespace LibraryManagementSystem;

public class AdministrationService
{
    private readonly List<LibrarianService> _librarians;

    public AdministrationService()
    {
        _librarians = new List<LibrarianService>();
    }

    public void AddLibrarian(LibrarianService librarian)
    {
        _librarians.Add(librarian);
    }

    public void RemoveLibrarian(LibrarianService librarian)
    {
        _librarians.Remove(librarian);
    }
}