
using ApiForBaseWeaknesses.Dto;
using ApiForBaseWeaknesses.Models;
using System.Text.Json.Serialization;

namespace ApiForBaseWeaknesses.Services;

public class BaseService
{
    private readonly AppDbContext _context;

    public BaseService(AppDbContext context)
    {
        _context = context;
    }
}