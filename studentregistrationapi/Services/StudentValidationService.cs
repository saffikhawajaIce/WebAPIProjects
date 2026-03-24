namespace studentregistrationapi;

public class StudentValidationService
{
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

        if (string.IsNullOrWhiteSpace(student.Email) || !student.Email.Contains("@"))
        {
            errors.Add("Valid email is required.");
        }

        if (student.Age < 16 || student.Age > 100)
        {
            errors.Add("Age must be between 16 and 100.");
        }

        if (string.IsNullOrWhiteSpace(student.Department))
        {
            errors.Add("Department is required.");
        }

        return errors.Count == 0;
    }
}