using System.IO;
using System.Linq;
using System.Collections.Generic;
namespace studentregistrationapi;

public class DataManagerService
{
    private readonly StudentValidationService studentValidationService;
    string filePath = "StudentList.txt";

    //local list to hold student data
    List<Student> students = new List<Student>();

    public DataManagerService(StudentValidationService studentValidationService)
    {
        this.studentValidationService = studentValidationService;
    }

    //this class can be used to manage data operations and validation logic in a more centralized way, if needed in the future. For now, it serves as a placeholder for potential future enhancements.
    //additional methods for complex data operations or combined logic can be added here as the application evolves.
    //this class will use the text file named StudentList.txt to store student data persistently. The file will be read and written to for all CRUD operations, ensuring that data is retained even when the application is restarted.
    //the text file is contained within the project directory and will be accessed using relative paths.
    //The file will be structured in a way that allows for easy parsing and management of student records, such as using a simple CSV format or JSON format for storing student data.
    //This approach allows for a lightweight and straightforward method of data persistence without the need for a full database setup, while still providing the necessary functionality for managing student records effectively.

    //basic read method to load students from the text file
    public List<Student> LoadStudentsFromFile()
    {
        //implementation to read from StudentList.txt and populate studentManagerService._students list
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            Student student = new Student
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                Email = parts[2],
                Age = int.Parse(parts[3]),
                Department = parts[4]
            };
            //i want this to return a list of students, so i will add the student object to the list of students in the data manager service,
            // and then return the list of students to the client
            students.Add(student);
        }
        return students;
    }

    //basic write method to save students to the text file
    public void SaveStudentsToFile(List<Student> students)
    {
        //implementation to write the current list of students from studentManagerService._students to StudentList.txt
        List<string> lines = new List<string>();
        foreach (Student student in students)
        {
            string line = $"{student.Id},{student.Name},{student.Email},{student.Age},{student.Department}";
            lines.Add(line);
        }
        File.WriteAllLines(filePath, lines);
    }

    //additional methods for complex data operations or combined logic can be added here as the application evolves.

}