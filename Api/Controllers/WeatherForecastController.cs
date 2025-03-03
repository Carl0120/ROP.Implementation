using Microsoft.AspNetCore.Mvc;
using ROP.Implementation.Result;
using ROP.Implementation.ResultExtensions;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    [HttpGet(Name = "Get")]
    public ResultAction<Unit> Get()
    {
        ResultAction result = ResultAction.Success("Creado Correctamente");

        return result.Ensure(e => e.Equals("Pedro"), new ErrorValidation("Error en la validacion"));
    }
}
