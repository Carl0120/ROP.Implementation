using Api.ResultExtensions;
using Microsoft.AspNetCore.Mvc;
using ROP.Implementation.Result;
using ROP.Implementation.ResultExtensions;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet(Name = "Get")]
    public IActionResult Get()
    {
        var result = ResultAction.Success();
        var res = result.Map(5).MatchToActionResult();
        return res;
    }
}
