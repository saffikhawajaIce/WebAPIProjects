namespace studentregistrationapi;

public class StudentManagerService
{
    //in-memory list to store students
    public List<Student> _students;
    private StudentValidationService studentValidationService;
    private DataManagerService dataManagerService;

    //constructor
    public StudentManagerService(StudentValidationService studentValidationService, DataManagerService dataManagerService)
    {
        _students = new List<Student>();
        this.studentValidationService = studentValidationService;
        this.dataManagerService = dataManagerService;
    }

    //basic CRUD operations
    public void AddStudent(Student student)
    {
        //validate student data before adding
        if (studentValidationService.ValidateStudent(student, out var errors))
        {
            _students.Add(student);
            dataManagerService.SaveStudentsToFile();
        }
        else
        {
            throw new ArgumentException(string.Join("; ", errors));
        }
    }

    public IEnumerable<Student> GetAllStudents()
    {
        //first try to load students from the text file, if the list is empty, then return the in-memory list
        List<Student> _students = dataManagerService.LoadStudentsFromFile();

        if (_students == null)
        {
            _students = new List<Student>();
        }
        else if (_students.Count == 0)
        {
            dataManagerService.SaveStudentsToFile();
        }

        return _students;
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
                dataManagerService.SaveStudentsToFile();
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
            dataManagerService.SaveStudentsToFile();
        }
    }
}