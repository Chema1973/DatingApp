using System.Text.Json;
using APIDatingApp.Helpers;

namespace APIDatingApp.Extensions
{
    public static class HttpExtensions
    {
        // PAGINACIÓN
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header){
            var jsonOptions = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            response.Headers.Add("Pagination", JsonSerializer.Serialize(header, jsonOptions));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}