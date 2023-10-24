namespace NamNamAPI.Controllers
{

    using Domain;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using NamNamAPI.Business;

    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {

        public ImageController()
        {
        }
        [HttpGet("{nombreArchivo}")]
        public IActionResult ObtenerImagen(string nombreArchivo)
        {
            // Lógica para devolver la imagen

            // Obtener la ruta completa del archivo
            var rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", nombreArchivo);

            // Verificar si el archivo existez
            if (System.IO.File.Exists(rutaArchivo))
            {
                // Leer el contenido del archivo
                var contenido = System.IO.File.ReadAllBytes(rutaArchivo);

                // Devolver el archivo como respuesta
                return File(contenido, "image/jpeg"); // Cambiar "image/jpeg" según el tipo de la imagen
            }
            else
            {
                // Si el archivo no existe, devolver un StatusCode 404 (No encontrado)
                return NotFound();
            }
        }
    }
}
