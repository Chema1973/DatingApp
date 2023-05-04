using APIDatingApp.Data;
using APIDatingApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIDatingApp.Controllers
{
    /// Pruebas de control de errores
    public class BuggyController : BaseApiController
    {
        
        private readonly DataContext _dataContext;

        public BuggyController(DataContext dataContext){
            _dataContext = dataContext;
        }
        
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _dataContext.Users.Find(-1);

            if (thing == null) return NotFound();

            return thing;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            // try {

                var thing = _dataContext.Users.Find(-1);

                var thingToReturn = thing.ToString();

                return thingToReturn;
            // Quitamos el try-catch para manejar excepciones a un nivel más alto
            // Cuando se produzca una excepción, será en "ExceptionMiddleware"
            /*}
            catch(Exception ex){

                return StatusCode(500, "Computer says no!!");
            }*/
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This is not a good request");
        }
    }

    

}