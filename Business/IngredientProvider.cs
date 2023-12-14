namespace NamNamAPI.Business
{
    using System.Globalization;
    using NamNamAPI.Domain;
    using NamNamAPI.Models;

    public class IngredientProvider
    {
        private NamnamContext connectionModel;

        public IngredientProvider(NamnamContext _connectionModel)
        {
            string Culture = "es-MX";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
            connectionModel = _connectionModel;
        }

        public (int, List<IngredientDomain>) getIngredients()
        {
            int code = 200;
            List<IngredientDomain> ingredientList = new List<IngredientDomain>();
            string report = "";
            try
            {
                var listTemp = connectionModel.Ingredients.ToList();
                foreach (var item in listTemp)
                {
                    IngredientDomain ingredient = new IngredientDomain
                    {
                        idIngredient = item.IdIngredient,
                        ingredientname = item.IngredientName,
                        measure = item.Measure
                    };
                    ingredientList.Add(ingredient);
                }
            }
            catch (Exception e)
            {
                code = 500;
                report = e.Message;
            }
            return (code, ingredientList);
        }

        public int setRecipeHasIngredients(List<Recipe_has_IngredientDomain> ingredientsList, string idRecipe)
        {
            int code = 200;
            try
            {
                List<RecipeHasIngredient> list = new List<RecipeHasIngredient>();
                foreach (var item in ingredientsList)
                {
                    RecipeHasIngredient recipeHasIngredient = new RecipeHasIngredient();
                    recipeHasIngredient.IngredientIdIngredient = item.ingredient_idIngredient;
                    recipeHasIngredient.RecipeIdRecipe = idRecipe;
                    recipeHasIngredient.Amount = item.amount;
                    list.Add(recipeHasIngredient);
                }
                connectionModel.RecipeHasIngredients.AddRange(list);
                int changes = connectionModel.SaveChanges();
                if (changes != ingredientsList.Count)
                    code = 500;
            }
            catch (Exception e)
            {
                code = 500;
            }
            return code;
        }

        public bool UpdateRecipeHasIngredients(List<Recipe_has_IngredientDomain> ingredientsList, string idRecipe)
        {
            bool result = false;
            try
            {
                List<RecipeHasIngredient> list = new List<RecipeHasIngredient>();
                foreach (var item in ingredientsList)
                {
                    RecipeHasIngredient recipeHasIngredient = new RecipeHasIngredient();
                    recipeHasIngredient.IngredientIdIngredient = item.ingredient_idIngredient;
                    recipeHasIngredient.RecipeIdRecipe = idRecipe;
                    recipeHasIngredient.Amount = item.amount;
                    list.Add(recipeHasIngredient);
                }
                connectionModel.RecipeHasIngredients.RemoveRange(connectionModel.RecipeHasIngredients.Where(a => a.RecipeIdRecipe == idRecipe));
                connectionModel.RecipeHasIngredients.AddRange(list);
                connectionModel.SaveChanges();
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
    }
}
