using System.Security.Claims;
using API.Data;
using API.Entities;
using API.Repository.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Repository;

public class ShortenRepository : IShortenRepository
{
    private readonly UrlContext _context;

    public ShortenRepository(UrlContext context)
    {
        _context = context;
    }

    public async Task<ShortenedUrl> Create(ShortenedUrl shortenedUrl)
    {
        _context.ShortenedUrls.Add(shortenedUrl);
        await _context.SaveChangesAsync();
        return shortenedUrl;
    }

    public async Task Delete(int id, ClaimsPrincipal user)
    {
        var isAdmin = user.IsInRole("Admin");
        var url = isAdmin 
            ? await _context.ShortenedUrls.FindAsync(id)
            : await _context.ShortenedUrls.FirstOrDefaultAsync(u => u.Id == id && u.CreatedBy == user.Identity.Name);

        if (url == null)
            throw new Exception($"You cannot do it");

        _context.ShortenedUrls.Remove(url);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ShortenedUrl>> GetAll()
    {
        return await _context.ShortenedUrls.ToListAsync();
    }
}