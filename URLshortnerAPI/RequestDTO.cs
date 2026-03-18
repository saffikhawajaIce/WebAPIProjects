
namespace URLshortnerAPI;

public class RequestDTO
{
    public string OriginalURL { get; set; }

    public RequestDTO(string originalURL)
    {
        OriginalURL = originalURL;
    }
}