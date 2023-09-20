using FileConsumer.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FileConsumer.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly BrokerContext _brokerContext;

    public FileController(BrokerContext brokerContext)
    {
        _brokerContext = brokerContext;
    }
    
    [HttpGet("messages")]
    public async Task<ActionResult> Messages()
    {
        var messages = await _brokerContext.Messages.ToListAsync();
        
        return Ok(messages);
    }
    
    
}