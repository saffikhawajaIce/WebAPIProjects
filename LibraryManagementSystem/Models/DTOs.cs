namespace LibraryManagementSystem;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class RegisterRequestDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }

    public RegisterRequestDTO() { }
}

public class LoginRequestDTO
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }

    public LoginRequestDTO() { }
}

public class AuthResponseDTO
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; }

    public AuthResponseDTO() { }
}

public class CreateBookDTO
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Author { get; set; }
    [Required]
    public DateTime PublishedDate { get; set; }

    public CreateBookDTO() { }
}

public class ResponseBookDTO
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("author")]
    public string Author { get; set; }

    [JsonPropertyName("publishedDate")]
    public string PublishedDate { get; set; }

    public ResponseBookDTO() { }
}

public class UpdateBookDTO
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Author { get; set; }
    [Required]
    public DateTime PublishedDate { get; set; }

    public UpdateBookDTO() { }
}