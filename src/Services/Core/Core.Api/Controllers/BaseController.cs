using Core.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Controllers;

[ApiController, ApiResult]
[Route("v{v:apiVersion}/[controller]")]
public class BaseController : ControllerBase
{

}

