
namespace URLshortnerAPI;

public class URLmodel
{
    public string OriginalURL { get; set; }

    public int URLId { get; set; }
    public string ShortenedURL { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ClickCount { get; set; } = 0;

    public URLmodel(string originalURL, int urlId, string shortenedURL, DateTime createdAt, int clickCount)
    {
        OriginalURL = originalURL;
        URLId = urlId;
        ShortenedURL = shortenedURL;
        CreatedAt = createdAt;
        ClickCount = clickCount;
    }
}