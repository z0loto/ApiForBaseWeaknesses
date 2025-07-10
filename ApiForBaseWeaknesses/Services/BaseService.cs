namespace ApiForBaseWeaknesses.Services;

public class BaseService
{
    private readonly AppDbContext _context;

    public BaseService(AppDbContext context)
    {
        _context = context;
    }
}