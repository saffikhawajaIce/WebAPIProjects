namespace LibraryManagementSystem;

public class PasswordValidationService
{
    public bool ValidatePassword(string password)
    {
        // Basic password validation rules:
        // - Minimum length of 8 characters
        // - At least one uppercase letter
        // - At least one digit
        // - At least one lowercase letter
        // - At least one special character

        if (string.IsNullOrWhiteSpace(password))
        {
            return false; // Password cannot be null or whitespace
        }
        if (string.IsNullOrEmpty(password) || password.Length < 8)
        {
            return false; // Password must be at least 8 characters long
        }

        if (!password.Any(char.IsUpper))
        {
            return false; // Password must contain at least one uppercase letter
        }

        if (!password.Any(char.IsDigit))
        {
            return false; // Password must contain at least one digit
        }

        if (!password.Any(char.IsLower))
        {
            return false; // Password must contain at least one lowercase letter
        }

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            return false; // Password must contain at least one special character
        }

        if (password.Contains(" "))
        {
            return false; // Password must not contain spaces
        }

        return true; // Password is valid
    }
}