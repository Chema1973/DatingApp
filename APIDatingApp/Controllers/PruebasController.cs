using APIDatingApp.Data;
using APIDatingApp.Interfaces;
using APIDatingApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIDatingApp.Controllers
{
    public class PruebasController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IPruebaService _pruebaService;
        private readonly ScopedPruebaService _scopedPruebaService;
        private readonly SingletonPruebaService _singletonPruebaService;
        private readonly TransientPruebaService _transientPruebaService;

        public PruebasController(DataContext context,
            IPruebaService pruebaService,
            ScopedPruebaService scopedPruebaService,
            SingletonPruebaService singletonPruebaService,
            TransientPruebaService transientPruebaService)
        {
            _context = context;
            _pruebaService = pruebaService;
            _scopedPruebaService = scopedPruebaService;
            _singletonPruebaService = singletonPruebaService;
            _transientPruebaService = transientPruebaService;
        }

        [HttpGet("getPruebasServicios")]
        public ActionResult<string> GetPruebasServicios(string entrada)
        {
            if (string.IsNullOrWhiteSpace(entrada)) entrada = "Chema";

            var resultado1 = _pruebaService.ObtenerScoped();
            var resultado2 = _pruebaService.ObtenerSingleton();
            var resultado3 = _pruebaService.ObtenerTransient();

            var resultadoSin = _singletonPruebaService.Guid;
            var resultadoSco = _scopedPruebaService.Guid;
            var resultadoTra = _transientPruebaService.Guid;

            var resultado = resultado1 + " \n\r " + resultado2 + " \n\r " + resultado3 + " \n\r " +
                resultadoSin + " \n\r " + resultadoSco + " \n\r " + resultadoTra;

            return resultado;
        }


        [HttpGet("getPruebas")]
        public ActionResult<string> GetPruebas(string entrada)
        {
            if (string.IsNullOrWhiteSpace(entrada)) entrada = "Chema";

            var resultado = _pruebaService.DevuelveMensaje(entrada);

            var resultadoSin = _singletonPruebaService.DevuelvePropiedadSingleton(entrada);
            var resultadoSco = _scopedPruebaService.DevuelvePropiedadScoped(entrada);
            var resultadoTra = _transientPruebaService.DevuelvePropiedadTransient(entrada);

            resultado = resultado + " \n\r " + resultadoSin + " \n\r " + resultadoSco + " \n\r " + resultadoTra;
            

            return resultado;
        }

        [HttpGet("getPruebasDobles")]
        public ActionResult<string> GetPruebasDobles(string entrada)
        {
            if (string.IsNullOrWhiteSpace(entrada)) entrada = "Chema";

            var resultado1 = _pruebaService.DevuelveMensaje(entrada);
            var resultado2 = _pruebaService.DevuelveMensaje(entrada);
            var resultado3 = _pruebaService.DevuelveMensaje(entrada);

            var resultadoSin1 = _singletonPruebaService.DevuelvePropiedadSingleton(entrada);
            var resultadoSin2 = _singletonPruebaService.DevuelvePropiedadSingleton(entrada);
            var resultadoSin3 = _singletonPruebaService.DevuelvePropiedadSingleton(entrada);
            var resultadoSco1 = _scopedPruebaService.DevuelvePropiedadScoped(entrada);
            var resultadoSco2 = _scopedPruebaService.DevuelvePropiedadScoped(entrada);
            var resultadoSco3 = _scopedPruebaService.DevuelvePropiedadScoped(entrada);
            var resultadoTra1 = _transientPruebaService.DevuelvePropiedadTransient(entrada);
            var resultadoTra2 = _transientPruebaService.DevuelvePropiedadTransient(entrada);
            var resultadoTra3 = _transientPruebaService.DevuelvePropiedadTransient(entrada);

            var resultado = resultado1 + " \n\r " + resultado2 + " \n\r " + resultado3 + " \n\r " +
                resultadoSin1 + " \n\r " + resultadoSin2 + " \n\r " + resultadoSin3 + " \n\r " + 
                resultadoSco1 + " \n\r " + resultadoSco2 + " \n\r " + resultadoSco3 + " \n\r " + 
                resultadoTra1 + " \n\r " + resultadoTra2 + " \n\r " + resultadoTra3;
            ;

            return resultado;
        }



        [HttpGet("getPrueba")]
        public ActionResult<string> GetPrueba()
        {

            var resultado = _pruebaService.DevuelveMensaje();

            resultado = resultado + _pruebaService.DevuelveMensaje();

            return resultado;
        }


        [HttpGet("getSingleton")]
        public ActionResult<string> GetSingleton()
        {

           var resultado = _singletonPruebaService.DevuelvePropiedadSingleton();

           return resultado;
        }

        [HttpGet("getTransient")]
        public ActionResult<string> GetTransient()
        {
            var resultado = _transientPruebaService.DevuelvePropiedadTransient();

            return resultado;
        }

        [HttpGet("getScoped")]
        public ActionResult<string> GetScoped()
        {
            var resultado = _scopedPruebaService.DevuelvePropiedadScoped();

            return resultado;
        }



    }
}