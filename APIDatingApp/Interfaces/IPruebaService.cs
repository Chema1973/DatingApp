namespace APIDatingApp.Interfaces
{
    public interface IPruebaService
    {
         string Mensaje {get; set;}

         string DevuelveMensaje();
         string DevuelveMensaje(string entrada);

         Guid ObtenerScoped();
        Guid ObtenerSingleton();
        Guid ObtenerTransient();
    }

    public interface IPruebaServiceTransient
    {
        string PropiedadTransient {get; set;}

         string DevuelvePropiedadTransient();
         string DevuelvePropiedadTransient(string entrada);

        
        
    }

    public interface IPruebaServiceScoped
    {
        string PropiedadScoped {get; set;}

         string DevuelvePropiedadScoped();
         string DevuelvePropiedadScoped(string entrada);

         
    }

    public interface IPruebaServiceSingleton
    {
        string PropiedadSingleton {get; set;}

         string DevuelvePropiedadSingleton();
         string DevuelvePropiedadSingleton(string entrada);
         
    }
}