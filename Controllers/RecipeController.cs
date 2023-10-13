using Microsoft.AspNetCore.Mvc;
using NamNamAPI.Business;
using NamNamAPI.Domain;
using System.Collections.Generic;

namespace NamNamAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController: ControllerBase
    {
        private RecipeProvider recipeProvider;
        public RecipeController(RecipeProvider _recipeProvider)
        {
            recipeProvider = _recipeProvider;
        }

        [HttpGet("GetCookbook/{idUser}")]
        public ActionResult GetCookbook(string idUser)
        {
            try
            {
                List<RecipeDomain> recipeList = recipeProvider.GetCookbook(idUser);

                if (recipeList == null || recipeList.Count == 0)
                {
                    return NotFound("No se encontraron recetas para este usuario.");
                }
                return Ok(recipeList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener recetas: {ex.Message}");

                return StatusCode(500, "Se produjo un error al procesar la solicitud.");
            }

        }
    }
}
