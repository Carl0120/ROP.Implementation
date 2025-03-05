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
       return ResultAction<int>
           .Success(5,"Creado Correctamente")
             .Map("Pedro")
             .Ensure(e => e == "Pedro"
                ,"Nombre"
                , "El tipo no se llama pedro");


    }
}
