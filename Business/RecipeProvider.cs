using NamNamAPI.Domain;
using NamNamAPI.Models;
using System.Globalization;

namespace NamNamAPI.Business
{
    public class RecipeProvider
    {
        private NamnamContext connectionModel;

        public RecipeProvider(NamnamContext _connectionModel)
        {
            string Culture = "es-MX";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
            connectionModel = _connectionModel;
        }

        public List<RecipeDomain> GetCookbook(string idUser)
        {
            List<RecipeDomain> recipeList = new List<RecipeDomain>();
            try
            {
                var listRecipe = connectionModel.Recipes.Where(recipe => recipe.UserIdUser == idUser).ToList();
                foreach (var item in listRecipe){
                    var recipe = new RecipeDomain();
                    recipe.idRecipe = item.IdRecipe;
                    recipe.User_idUser = item.UserIdUser;
                    recipe.recipeName = item.ReceipName;
                    recipe.preparationTime = item.PreparationTime.ToString();
                    recipe.idMainIngredient = item.IdMainIngredient;
                    recipeList.Add(recipe);
                }
            }
            catch (Exception e)
            {
                throw new ExceptionBusiness("Error al obtener recetas: " + e.Message);
            }
            return recipeList;
        }
    }
}
