using Apcitas.WebService.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Apcitas.WebService.Controllers;
[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
}
