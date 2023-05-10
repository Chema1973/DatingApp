using APIDatingApp.Extensions;
using APIDatingApp.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APIDatingApp.Helpers
{
    // Por medio de este filtro de acción, se actualizará para cada usuario su fecha de última actividad
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;
            // --> Si el usuario no está autenticado no haremos nada

            var userId = resultContext.HttpContext.User.GetUserId();

            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            // --> Podemos coger el repositorio que queramos
            var user = await repo.GetUserByIdAsync(userId);
            user.LastActive = DateTime.UtcNow;
            await repo.SaveAllAsync();
        }
    }
}