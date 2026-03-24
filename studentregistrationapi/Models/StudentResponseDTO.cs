namespace studentregistrationapi;

public class StudentResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string Department { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public bool IsEnrolled { get; set; }

    //default constructor for model binding and serialization
    public StudentResponseDTO()
    {

    }

    public StudentResponseDTO(int id, string name, string email, int age, string department, DateTime enrollmentDate, bool isEnrolled)
    {
        Id = id;
        Name = name;
        Email = email;
        Age = age;
        Department = department;
        EnrollmentDate = enrollmentDate;
        IsEnrolled = isEnrolled;
    }
}