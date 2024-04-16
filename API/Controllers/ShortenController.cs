using System.Security.Claims;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Repository;
using API.Repository.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ShortenController : ControllerBase
{
    private readonly UrlContext _context;
    private readonly UrlShorteningService _urlShorteningService;
    private readonly IShortenRepository _shortenRepository;

    public ShortenController(UrlContext context, UrlShorteningService urlShorteningService,
        IShortenRepository shortenRepository)
    {
        _context = context;
        _urlShorteningService = urlShorteningService;
        _shortenRepository = shortenRepository;
    }

    [HttpPost("shorter")]
    public async Task<ActionResult> CreateShortUrl(ShortenUrlRequestDto urlRequest)
    {
        if (!Uri.TryCreate(urlRequest.Url, UriKind.Absolute, out _))
            return BadRequest("The specified URL is invalid");

        var isUrlExist = await _context.ShortenedUrls
            .FirstOrDefaultAsync(u => u.LongUrl == urlRequest.Url);

        if (isUrlExist != null)
            return BadRequest("The specified URL already exist");

        var code = await _urlShorteningService.GenerateUniqueCode();

        var shortenedUrl = new ShortenedUrl
        {
            LongUrl = urlRequest.Url,
            Code = code,
            ShortUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/{code}",
            CreatedData = DateTime.Now,
            CreatedBy = User.Identity.Name
        };

        var createdUrl = await _shortenRepository.Create(shortenedUrl);

        return Ok(createdUrl.ShortUrl);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteShortenUrl(int id)
    {
        await _shortenRepository.Delete(id, User);

        return Ok();
    }

    [HttpGet("getAllUrls")]
    public async Task<List<ShortenedUrl>> GetAllShortenedUrl()
    {
        return await _shortenRepository.GetAll();
    }

    [AllowAnonymous]
    [HttpGet("/api/{code}")]
    public async Task<ActionResult> RedirectShortenUrl(string code)
    {
        var shortenedUrl = await _context.ShortenedUrls
            .SingleOrDefaultAsync(o => o.Code == code);

        if (shortenedUrl == null)
            return NotFound();

        return Redirect(shortenedUrl.LongUrl);
    }
}