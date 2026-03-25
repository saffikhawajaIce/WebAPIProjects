// namespace LibraryManagementSystem;

// public class StudentManagementService
// {
//     private readonly List<Student> _students;

//     public StudentManagementService()
//     {
//         _students = new List<Student>();
//     }

//     public List<Student> GetAllStudents()
//     {
//         return _students;
//     }

//     public Student GetStudentById(int id)
//     {
//         return _students.FirstOrDefault(s => s.Id == id);
//     }

//     public void AddStudent(Student student)
//     {
//         _students.Add(student);
//     }

//     public void UpdateStudent(Student updatedStudent)
//     {
//         var existingStudent = GetStudentById(updatedStudent.Id);
//         if (existingStudent != null)
//         {
//             existingStudent.Name = updatedStudent.Name;
//             existingStudent.Email = updatedStudent.Email;
//             existingStudent.DateOfBirth = updatedStudent.DateOfBirth;
//         }
//     }

//     public void DeleteStudent(int id)
//     {
//         var studentToRemove = GetStudentById(id);
//         if (studentToRemove != null)
//         {
//             _students.Remove(studentToRemove);
//         }
//     }
// }