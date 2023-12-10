using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using NamNamAPI.Domain;
using NamNamAPI.Models;
using Newtonsoft.utility;
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
                foreach (var item in listRecipe)
                {
                    var recipe = new RecipeDomain();
                    recipe.idRecipe = item.IdRecipe;
                    recipe.User_idUser = item.UserIdUser;
                    recipe.recipeName = item.ReceipName;
                    recipe.imageRecipeURL = item.ImageRecipeUrl;
                    recipe.preparationTime = item.PreparationTime.ToString();
                    recipe.idMainIngredient = item.IdMainIngredient;
                    recipe.Portion = item.Portion;
                    recipeList.Add(recipe);
                }
            }
            catch (Exception e)
            {
                throw new ExceptionBusiness("Error al obtener recetas: " + e.Message);
            }
            return recipeList;
        }

        public List<RecipeDomain> GetFavoriteRecipes(string idUser)
        {
            List<RecipeDomain> recipeList = new List<RecipeDomain>();
            try
            {
                var listFavoriteRecipe = connectionModel.Recipes
                    .Include(r => r.IdUserFavorites)
                    .Where(s => s.IdUserFavorites.Equals(idUser))
                    .ToList();
                foreach (var item in listFavoriteRecipe)
                {
                    var recipe = new RecipeDomain();
                    recipe.idRecipe = item.IdRecipe;
                    recipe.User_idUser = item.UserIdUser;
                    recipe.recipeName = item.ReceipName;
                    recipe.imageRecipeURL = item.ImageRecipeUrl;
                    recipe.preparationTime = item.PreparationTime.ToString();
                    recipe.idMainIngredient = item.IdMainIngredient;
                    recipeList.Add(recipe);
                }
            }
            catch (Exception e)
            {
                throw new ExceptionBusiness("Error al obtener recetas favoritas: " + e.Message);
            }
            return recipeList;
        }

        public (int, string, string) PostRecipe(RecipeDomain newRecipe, CategoryDomain categoryDomain)
        {
            string errormsg = "";
            int changes = 0;
            string idRecipeNew = GenerateRandomID.GenerateID();


            try
            {


                // Verificar si los bytes de la imagen no son nulos y tienen contenido.
                if (newRecipe.ImageBytes != null && newRecipe.ImageBytes.Length > 0)
                {
                    // Ruta de la carpeta donde se guardarán las imágenes (debes ajustarla según tu proyecto).
                    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");

                    // Verificar si la carpeta existe; si no, créala.
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Generar un nombre de archivo único para evitar colisiones.
                    string fileName = Guid.NewGuid().ToString() + ".jpg";
                    string filePath = Path.Combine(folderPath, fileName);

                    // Guardar los bytes en el archivo.
                    File.WriteAllBytes(filePath, newRecipe.ImageBytes);
                }

                    ///////////////////////////
                    Category categoryModel = connectionModel.Categories.Find(categoryDomain.idCategory);
                    if (categoryModel != null)
                    {
                        Recipe recipeTemp = new Recipe
                        {
                            IdRecipe = idRecipeNew,
                            UserIdUser = newRecipe.User_idUser,
                            ReceipName = newRecipe.recipeName,
                            ImageRecipeUrl = "https://nam-nam-api.azurewebsites.net/Image/poz.jpg",
                            PreparationTime = TimeOnly.Parse("00:00:00"),
                            IdMainIngredient = newRecipe.idMainIngredient,
                            Portion = newRecipe.Portion,
                            IsEnable = true
                            
                        };
                        recipeTemp.CategoryIdCategories.Add(categoryModel);
                        connectionModel.Recipes.Add(recipeTemp);
                        changes = connectionModel.SaveChanges();
                    }
                

            }
            catch (Exception e)
            {
                changes = 500;
                errormsg = e.StackTrace;
            }
            return (changes, idRecipeNew, errormsg);
        }

        public (int, GetRecipeResponse) getRecipe(string idRecipe)
        {
            int code = 0;
            GetRecipeResponse response = new GetRecipeResponse();
            try
            {
                //RECIPE
                Recipe recipeModel = connectionModel.Recipes.Include(r => r.CategoryIdCategories).FirstOrDefault(r => r.IdRecipe == idRecipe);
                var categoryRecipeList = recipeModel.CategoryIdCategories;
                //CATEGORY
                // Category categoryModel = connectionModel.Categories.Find(categoryRecipeList[0].category.);
                //STEPS
                var stepListModel = connectionModel.Cookinginstructions.Where(a => a.RecipeIdRecipe == idRecipe).ToList();
                //INGREDIENTS
                var ingredientListModel = connectionModel.Ingredients.ToList();
                var recipeHasIngredientsListModel = connectionModel.RecipeHasIngredients.Where(a => a.RecipeIdRecipe == idRecipe).ToList();
                if (recipeModel != null)
                {

                    //CONFIGURANDO RESPUESTA
                    RecipeDomain recipeTemp = new RecipeDomain();
                    recipeTemp.idRecipe = recipeModel.IdRecipe;
                    recipeTemp.recipeName = recipeModel.ReceipName;
                    recipeTemp.User_idUser = recipeModel.UserIdUser;
                    recipeTemp.imageRecipeURL = recipeModel.ImageRecipeUrl;
                    recipeTemp.preparationTime = recipeModel.PreparationTime.ToString();
                    recipeTemp.idMainIngredient = recipeModel.IdMainIngredient;
                    recipeTemp.Portion = recipeModel.Portion;
                    var category = recipeModel.CategoryIdCategories.ToList()[0];
                    CategoryDomain categoryTemp = new CategoryDomain();
                    categoryTemp.idCategory = category.IdCategory;
                    categoryTemp.categoryName = category.CategoryName;

                    //LISTA DE PASOS
                    List<CookinginstructionDomain> instructionTempList = new List<CookinginstructionDomain>();
                    foreach (var item in stepListModel)
                    {
                        Console.WriteLine("paso: " + item.Instruction);
                        CookinginstructionDomain step = new CookinginstructionDomain();
                        step.IdCookingInstruction = item.IdCookingInstruction;
                        step.Instruction = item.Instruction;
                        step.RecipeIdRecipe = item.RecipeIdRecipe;
                        step.Step = item.Step;
                        instructionTempList.Add(step);
                    }
                    //LISTA DE INGREDIENTS
                    List<Recipe_has_IngredientDomain> amountTempList = new List<Recipe_has_IngredientDomain>();
                    List<IngredientDomain> ingredientsTempList = new List<IngredientDomain>();
                    List<string> idIngredientList = new List<string>();
                    foreach (var ingredientItem in ingredientListModel)
                    {
                        foreach (var amountTemp in recipeHasIngredientsListModel)
                        {
                            if (amountTemp.IngredientIdIngredient == ingredientItem.IdIngredient)
                            {
                                IngredientDomain ingredientTemp = new IngredientDomain();//contiene el nombre y la unidad
                                Recipe_has_IngredientDomain recipeHasIngredientTemp = new Recipe_has_IngredientDomain();//contiene la cantidad
                                ingredientTemp.idIngredient = ingredientItem.IdIngredient;
                                ingredientTemp.ingredientname = ingredientItem.IngredientName;
                                ingredientTemp.measure = ingredientItem.Measure;
                                Recipe_has_IngredientDomain recipe_Has_IngredientDomainTemp = new Recipe_has_IngredientDomain();
                                recipe_Has_IngredientDomainTemp.Recipe_idRecipe = amountTemp.RecipeIdRecipe;
                                recipe_Has_IngredientDomainTemp.Ingredient_idIngredient = amountTemp.IngredientIdIngredient;
                                recipe_Has_IngredientDomainTemp.Amount = amountTemp.Amount;
                                amountTempList.Add(recipe_Has_IngredientDomainTemp);
                                ingredientsTempList.Add(ingredientTemp);
                                idIngredientList.Add(ingredientTemp.idIngredient);//usado para recuperar sus informacion nutricional

                            }
                        }
                    }
                    //LISTA DE INFORMACION NUTRICIONAL
                    List<NutritionalDataDomain> nutritionalDataDomainsList = new List<NutritionalDataDomain>();
                    var nutritionalInfo = connectionModel.Nutritionaldata.Where(a => idIngredientList.Contains(a.IngredientIdIngredient)).ToList();
                    foreach (var dataItem in nutritionalInfo)
                    {
                        NutritionalDataDomain nutritionalDataDomain = new NutritionalDataDomain();
                        nutritionalDataDomain.IdNutritionalData = dataItem.IdNutritionalData;
                        nutritionalDataDomain.Calories = dataItem.Calories;
                        nutritionalDataDomain.Fat = dataItem.Fat;
                        nutritionalDataDomain.Protein = dataItem.Protein;
                        nutritionalDataDomain.Carbohydrates = dataItem.Carbohydrates;
                        nutritionalDataDomain.IngredientIdIngredient = dataItem.IngredientIdIngredient;
                        nutritionalDataDomainsList.Add(nutritionalDataDomain);
                    }

                    response.recipe = recipeTemp;
                    response.category = categoryTemp;
                    response.stepList = new List<CookinginstructionDomain>();
                    response.stepList = instructionTempList;
                    response.ingredientList = new List<IngredientDomain>();
                    response.ingredientList = ingredientsTempList;
                    response.ingredientAmounList = new List<Recipe_has_IngredientDomain>();
                    response.ingredientAmounList = amountTempList;
                    response.nutritionalDataList = new List<NutritionalDataDomain>();
                    response.nutritionalDataList = nutritionalDataDomainsList;
                    code = 200;
                }

            }
            catch (Exception e)
            {
                code = 500;
                Console.WriteLine("EROR---------------" + e);
            }
            return (code, response);
        }


        public List<RecipeDomain> getRecipeTest()
        {
            List<RecipeDomain> listDomain = new List<RecipeDomain>();
            GetRecipeResponse response = new GetRecipeResponse();

            //RECIPE
            List<Recipe> recipeModel = connectionModel.Recipes.ToList();
            foreach (var item in recipeModel)
            {
                RecipeDomain x = new RecipeDomain();
                x.idRecipe = item.IdRecipe;
                x.recipeName = item.ReceipName;
                listDomain.Add(x);
            }

            return listDomain;
        }

        public bool EditRecipe(NewRecipeDomain newRecipe)
        {
            var result = false;
            try
            {
                Recipe recipeModel = connectionModel.Recipes
                .Include(r => r.CategoryIdCategories)
                .Include(s => s.Cookinginstructions)
                .Include(i => i.RecipeHasIngredients)
                .FirstOrDefault(r => r.IdRecipe == newRecipe.recipeDomain.idRecipe);

                if (recipeModel != null)
                {
                    if (recipeModel.Portion != newRecipe.recipeDomain.Portion)
                        recipeModel.Portion = newRecipe.recipeDomain.Portion;
                    if (recipeModel.ReceipName != newRecipe.recipeDomain.recipeName)
                        recipeModel.ReceipName = newRecipe.recipeDomain.recipeName;
                    if (recipeModel.IdMainIngredient != newRecipe.recipeDomain.idMainIngredient)
                        recipeModel.IdMainIngredient = newRecipe.recipeDomain.idMainIngredient;

                    foreach (var category in recipeModel.CategoryIdCategories.ToList())
                    {
                        recipeModel.CategoryIdCategories.Remove(category);
                    }
                    Category categoryModel = connectionModel.Categories.Find(newRecipe.category.idCategory);
                        recipeModel.CategoryIdCategories.Add(categoryModel);
                }
                    connectionModel.SaveChanges();

                    //DeleteImage(recipeModel.ImageRecipeUrl);
                    //agregar imagen
                    //guardar url de la imagen
                    result = true;
            }
            catch (Exception e)
            {
                throw new ExceptionBusiness("Error al editar receta: " + e.Message);
            }
            return result;
        }
        public void DeleteImage(string path)
        {
            try
            {
                // Verifica si el archivo existe antes de intentar eliminarlo
                if (File.Exists(path))
                {
                    // Elimina el archivo
                    File.Delete(path);
                    Console.WriteLine($"Archivo {path} eliminado correctamente.");
                }
                else
                {
                    Console.WriteLine($"El archivo {path} no existe.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el archivo: {ex.Message}");
            }
        }
    }
}
