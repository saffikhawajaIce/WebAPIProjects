namespace studentregistrationapi;

public class StudentManagerService
{
    //in-memory list to store students
    public List<Student> _students;
    private StudentValidationService studentValidationService;

    //constructor
    public StudentManagerService(StudentValidationService studentValidationService)
    {
        _students = new List<Student>();
        this.studentValidationService = studentValidationService;
    }

    //basic CRUD operations
    public void AddStudent(Student student)
    {
        //validate student data before adding
        if (studentValidationService.ValidateStudent(student, out var errors))
        {
            _students.Add(student);
        }
        else
        {
            throw new ArgumentException(string.Join("; ", errors));
        }
    }

    public IEnumerable<Student> GetAllStudents()
    {
        //if no students are found, return an empty list
        return _students ?? new List<Student>();
    }

    public Student GetStudentById(int id)
    {
        return _students.FirstOrDefault(s => s.Id == id);
    }

    public void UpdateStudent(Student updatedStudent)
    {
        var existingStudent = GetStudentById(updatedStudent.Id);
        if (existingStudent != null)
        {
            if (studentValidationService.ValidateStudent(updatedStudent, out var errors))
            {
                existingStudent.Name = updatedStudent.Name;
                existingStudent.Email = updatedStudent.Email;
                existingStudent.Age = updatedStudent.Age;
                existingStudent.Department = updatedStudent.Department;
                existingStudent.EnrollmentDate = updatedStudent.EnrollmentDate;
                existingStudent.IsEnrolled = updatedStudent.IsEnrolled;
            }
            else
            {
                throw new ArgumentException(string.Join("; ", errors));
            }
        }
    }

    public void DeleteStudent(int id)
    {
        var studentToRemove = GetStudentById(id);
        if (studentToRemove != null)
        {
            _students.Remove(studentToRemove);
        }
    }
}