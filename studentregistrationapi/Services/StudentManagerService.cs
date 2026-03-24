using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace studentregistrationapi;

public class StudentManagerService
{
    //in-memory list to store students
    public List<Student> _students;

    //list to hold student response DTOs for returning to the client
    List<StudentResponseDTO> studentResponseDTOs = new List<StudentResponseDTO>();
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

    public IEnumerable<StudentResponseDTO> GetAllStudents()
    {
        //first try to load students from the text file, if the list is empty, then return the in-memory list
        List<Student> _students = dataManagerService.LoadStudentsFromFile();

        //i need to use the studentresponseDTO to return the student data to the client, so i will create a new list of studentresponseDTO objects and map the properties from the student objects to the studentresponseDTO objects, then return the list of studentresponseDTO objects to the client

        foreach (Student student in _students)
        {
            StudentResponseDTO studentResponseDTO = new StudentResponseDTO
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Age = student.Age,
                Department = student.Department,
                EnrollmentDate = student.EnrollmentDate,
                IsEnrolled = student.IsEnrolled
            };
            studentResponseDTOs.Add(studentResponseDTO);
        }

        if (_students == null)
        {
            _students = new List<Student>();
        }
        else if (_students.Count == 0)
        {
            dataManagerService.SaveStudentsToFile();
        }

        return studentResponseDTOs;
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