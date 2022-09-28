using Import.Logic.Storage;

using Microsoft.AspNetCore.Mvc;

namespace Import.Controllers;

[ApiController]
[Route("[controller]")]
public class ImportController : ControllerBase
{
    private readonly ILogger<ImportController> _logger;
    private readonly ImportContext _context;

    public ImportController(ILogger<ImportController> logger, ImportContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IEnumerable<string> Get() => _context.ExternalProducts.Select(x => x.ProductName).ToList()!;
}