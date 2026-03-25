namespace LibraryManagementSystem;

public class BorrowRecord
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int StudentId { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    //lol smart since is return can only be true if return date has value lololololololololol
    public bool IsReturned => ReturnDate.HasValue;

    public BorrowRecord() { }

    public BorrowRecord(int id, int bookId, int studentId, DateTime borrowDate, DateTime? returnDate)
    {
        Id = id;
        BookId = bookId;
        StudentId = studentId;
        BorrowDate = borrowDate;
        ReturnDate = returnDate;
    }
}