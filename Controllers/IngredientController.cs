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
    public class IngredientController : ControllerBase
    {
        private IngredientProvider ingredientProvider;
        public IngredientController(IngredientProvider _ingredientProvider)
        {
            ingredientProvider = _ingredientProvider;
        }

        [HttpGet("GetIngredients")]
        public ActionResult GetIngredients()
        {
            (int code, List<IngredientDomain> ingredientList) = ingredientProvider.getIngredients();
            if (code == 200)
            {
                return Ok(
                    ingredientList
                );
            }
            else
            {
                return NotFound();
            }


        }
    }
}