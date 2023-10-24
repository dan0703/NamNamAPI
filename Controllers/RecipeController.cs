using Microsoft.AspNetCore.Mvc;
using NamNamAPI.Business;
using NamNamAPI.Domain;
using System.Collections.Generic;

namespace NamNamAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private RecipeProvider recipeProvider;
        private InstructionProvider instructionProvider;
        public RecipeController([FromBody]RecipeProvider _recipeProvider, [FromBody]InstructionProvider _instructionProvider)
        {
            recipeProvider = _recipeProvider;
            instructionProvider = _instructionProvider;
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

        [HttpPost("PostRecipe")]
        public ActionResult PostRecipe([FromBody] NewRecipeDomain newRecipeDomain)
        {
            int codeInstruction = 0;
            (int codeRecipe, string idRecipe,string error) = recipeProvider.PostRecipe(newRecipeDomain.recipeDomain);
            if (codeRecipe == 1)
            {
                codeInstruction = instructionProvider.PostInstruction(newRecipeDomain.instructions,idRecipe);
                if (codeInstruction == newRecipeDomain.instructions.Count)
                    return Ok();
            }
            return StatusCode(500, error);

        }
    }
}
