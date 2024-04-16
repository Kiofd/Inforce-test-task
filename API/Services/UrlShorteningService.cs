using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class UrlShorteningService
{
    private readonly UrlContext _context;
    
    public const int NumberofCharsInShortLink = 7;
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private readonly Random _random = new();

    public UrlShorteningService(UrlContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateUniqueCode()
    {
        var codeChars = new char[NumberofCharsInShortLink];

        while (true)
        {
            for (int i = 0; i < NumberofCharsInShortLink; i++)
            {
                var randomIndex = _random.Next(Alphabet.Length - 1);

                codeChars[i] = Alphabet[randomIndex];
            }

            var code = new string(codeChars);

            if (await _context.ShortenedUrls.AnyAsync(s => s.Code == code))
            {
                return code;
            }

            return code;
        }
    }
}