using APIDatingApp.Interfaces;

namespace APIDatingApp.Services
{
    public class PruebaService : IPruebaService
    {
        // public string Mensaje { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        private readonly SingletonPruebaService _singletonPruebaService;
        private readonly TransientPruebaService _transientPruebaService;
        private readonly ScopedPruebaService _scopedPruebaService;

        public PruebaService (
            SingletonPruebaService singletonPruebaService,
            TransientPruebaService transientPruebaService,
            ScopedPruebaService scopedPruebaService
        ) {
            _singletonPruebaService = singletonPruebaService;
            _transientPruebaService = transientPruebaService;
            _scopedPruebaService = scopedPruebaService;
        }
        public string Mensaje { get; set; }

        public Guid Guid = Guid.NewGuid();
       

        public string DevuelveMensaje()
        {

            var respuesta = DateTime.Now.ToString("hh:mm:ss") + ":::Pruebas Service:::" + Guid;
            return respuesta;
        }

         public string DevuelveMensaje(string entrada)
        {
            var respuesta = DevuelveMensaje() + " //// " + entrada;
            respuesta = respuesta + "\r\nScoped::" + ObtenerScoped()
             + "\r\nTransient::" + ObtenerTransient()
              + "\r\nSingleton::" + ObtenerSingleton();
            return respuesta;
        }

        public Guid ObtenerScoped()
        {
            return _scopedPruebaService.Guid;
        }

        public Guid ObtenerSingleton()
        {
            return _singletonPruebaService.Guid;
        }

        public Guid ObtenerTransient()
        {
            return _transientPruebaService.Guid;
        }
    }

    public class SingletonPruebaService : IPruebaServiceSingleton
    {
        public string PropiedadSingleton { get; set; }

        public Guid Guid = Guid.NewGuid();

        
        public string DevuelvePropiedadSingleton()
        {
            var respuesta = DateTime.Now.ToString("hh:mm:ss") + ":::Pruebas Singleton:::" + Guid;
            return respuesta;
        }

        public string DevuelvePropiedadSingleton(string entrada)
        {
            var respuesta = DevuelvePropiedadSingleton() + " //// " + entrada;
            return respuesta;
        }

    }

    public class TransientPruebaService : IPruebaServiceTransient
    {
        public string PropiedadTransient { get; set; }

        public Guid Guid = Guid.NewGuid();

        public string DevuelvePropiedadTransient()
        {
            var respuesta = DateTime.Now.ToString("hh:mm:ss") + ":::Pruebas Transient:::" + Guid;
            return respuesta;
        }

        public string DevuelvePropiedadTransient(string entrada)
        {
            var respuesta = DevuelvePropiedadTransient() + " //// " + entrada;
            return respuesta;
        }


    }

    public class ScopedPruebaService : IPruebaServiceScoped
    {
        public string PropiedadScoped { get; set; }

        public Guid Guid = Guid.NewGuid();

        public string DevuelvePropiedadScoped()
        {
            var respuesta = DateTime.Now.ToString("hh:mm:ss") + ":::Pruebas Scoped:::" + Guid;
            return respuesta;
        }

        public string DevuelvePropiedadScoped(string entrada)
        {
            var respuesta = DevuelvePropiedadScoped() + " //// " + entrada;
            return respuesta;
        }

       
    }
}
/*

public ActionResult ObtenerGuids()
{
    return Ok(new
    {
        AutoresController_Transient = _servicioTransient.Guid,
        AutoresController_Scoped = _servicioScoped.Guid,
        AutoresController_Singleton = _servicioSingleton.Guid,
        ServicioA_Transient = _servicio.ObtenerTransient(),
        ServicioA_Scoped = _servicio.ObtenerScoped(),
        ServicioA_Singleton = _servicio.ObtenerSingleton()
    });

    // Petición A
    //  Transient distintos
    //  Scoped iguales
    //  Singleton iguales
    // Petición B
    //  Transient distintos (y distintos que Petición A)
    //  Scoped iguales (y distintos que Petición A)
    //  Singleton iguales (e iguales que Petición A)

    // --> El valor de transient siempre será dintinto aún en las mismas peticiones
    // --> El valor de scoped siempre será el mismo en una petición y distinto por peticiones
    // --> El valor de Singleton será siempre el mismo en todas las peticiones


}

*/