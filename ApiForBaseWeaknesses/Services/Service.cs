namespace ApiForBaseWeaknesses.Services;

public class Service
{
    private readonly AppDbContext _context;

    public Service(AppDbContext context)
    {
        _context = context;
    }
}