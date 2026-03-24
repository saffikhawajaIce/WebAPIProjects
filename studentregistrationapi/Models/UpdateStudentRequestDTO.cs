namespace studentregistrationapi;

public class UpdateStudentRequestDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string Department { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public bool IsEnrolled { get; set; }

    //default constructor for model binding and serialization
    public UpdateStudentRequestDTO()
    {

    }

    //the user does not need to send enrollment date and isEnrolled status in the request body, so i will set default values for those parameters in this constructor
    public UpdateStudentRequestDTO(int id, string name, string email, int age, string department, DateTime enrollmentDate = default, bool isEnrolled = true)
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