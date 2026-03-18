public class Validator
{
    public bool IsValidURL(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

}