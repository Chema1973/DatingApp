using APIDatingApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace APIDatingApp.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))] // Añadimos el filtro de actividad para el usuario
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {

    }
}