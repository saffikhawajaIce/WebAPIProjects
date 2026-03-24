using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace studentregistrationapi;

public class StudentValidationService
{
    private const int MinAge = 16;
    private const int MaxAge = 100;
    public StudentValidationService()
    {

    }
    //validation logic for student data
    public bool ValidateStudent(Student student, out List<string> errors)
    {
        errors = new List<string>();

        if (string.IsNullOrWhiteSpace(student.Name))
        {
            errors.Add("Name is required.");
        }

        else if (student.Name.Length < 2 || student.Name.Length > 50)
        {
            errors.Add("Name must be between 2 and 50 characters.");
        }

        //validate email format using System.Net.Mail.MailAddress
        try { _ = new System.Net.Mail.MailAddress(student.Email); }
        catch { errors.Add("Valid email is required."); }

        if (student.Age < MinAge || student.Age > MaxAge)
        {
            errors.Add($"Age must be between {MinAge} and {MaxAge}.");
        }

        if (string.IsNullOrWhiteSpace(student.Department))
        {
            errors.Add("Department is required.");
        }

        return errors.Count == 0;
    }
}