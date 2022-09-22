using Microsoft.AspNetCore.Mvc;

namespace Import.Controllers;

[ApiController]
[Route("[controller]")]
public class ImportController : ControllerBase
{
    private readonly ILogger<ImportController> _logger;

    public ImportController(ILogger<ImportController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<string> Get() => throw new NotImplementedException();
}