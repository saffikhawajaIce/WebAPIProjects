namespace studentregistrationapi;


public class CreateStudentRequestDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string Department { get; set; }

    //default constructor for model binding and serialization
    public CreateStudentRequestDTO()
    {

    }

    public CreateStudentRequestDTO(string name, string email, int age, string department)
    {
        Name = name;
        Email = email;
        Age = age;
        Department = department;
    }
}