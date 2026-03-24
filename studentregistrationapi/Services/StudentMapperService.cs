namespace studentregistrationapi;

public class StudentMapperService
{
    //basically i want to use this class and its methods for manual mapping instead of having mapping logic live in the endpoints.
    //then i can call this class's methods in the endpoint instead of defining logic there

    public StudentMapperService()
    {

    }

    //basic mapping methods to convert between Student and StudentResponseDTO, and between CreateStudentRequestDTO and Student.
    //These methods will be used in the endpoints to map data between the different layers of the application,
    //ensuring a clear separation of concerns and keeping the mapping logic centralized in one place for easier maintenance and readability.
    public StudentResponseDTO MapToStudentResponseDTO(Student student)
    {
        return new StudentResponseDTO
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Age = student.Age,
            Department = student.Department,
            EnrollmentDate = student.EnrollmentDate,
            IsEnrolled = student.IsEnrolled
        };
    }

    public Student MapToStudent(CreateStudentRequestDTO createStudentRequestDTO, int newId)
    {
        return new Student
        {
            Id = newId,
            Name = createStudentRequestDTO.Name,
            Email = createStudentRequestDTO.Email,
            Age = createStudentRequestDTO.Age,
            Department = createStudentRequestDTO.Department,
            EnrollmentDate = DateTime.Now,
            IsEnrolled = true
        };
    }

    //additional mapping methods can be added here as needed for other DTOs or data transformations in the future.


}