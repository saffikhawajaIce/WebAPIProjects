namespace URLshortnerAPI;

public class ValidatorService
{
    public bool IsValidURL(string url)
    {
        // Use Uri.TryCreate to validate the URL
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

}