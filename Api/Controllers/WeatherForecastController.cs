using Microsoft.AspNetCore.Mvc;
using ROP.Implementation.Result;
using ROP.Implementation.ResultExtensions;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    [HttpGet(Name = "Get")]
    public ResultAction<string> Get()
    {
        ResultAction<int> result = ResultAction<int>.Success(5,"Creado Correctamente");

        ResultAction<string> res = result.Map("Pedro");

        ResultAction<string> va =res.Ensure(e => e.Equals("Pedro"), "El tipo no se llama Juan");

        return va;
    }
}
