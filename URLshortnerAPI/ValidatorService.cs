namespace URLshortnerAPI;

public class ValidatorService
{
    public bool IsValidURL(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

}