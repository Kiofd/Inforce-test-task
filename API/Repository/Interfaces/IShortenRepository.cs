using System.Security.Claims;
using API.Dtos;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Repository.Interfaces;

public interface IShortenRepository
{
    Task<ShortenedUrl> Create(ShortenedUrl shortenedUrl);
    Task Delete(int id, ClaimsPrincipal user);
    Task<List<ShortenedUrl>> GetAll();
}