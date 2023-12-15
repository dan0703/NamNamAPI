using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using NamNamAPI.Domain;
using NamNamAPI.Models;
using Newtonsoft.utility;
using System.Globalization;
using System.Drawing;
using System.IO;

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
                    recipe.user_idUser = item.UserIdUser;
                    recipe.recipeName = item.ReceipName;
                    recipe.imageRecipeURL = item.ImageRecipeUrl;
                    recipe.preparationTime = item.PreparationTime.ToString();
                    recipe.idMainIngredient = item.IdMainIngredient;
                    recipe.portion = item.Portion;
                    recipeList.Add(recipe);
                }
            }
            catch (Exception e)
            {
                throw new ExceptionBusiness("Error al obtener recetas: " + e.Message);
            }
            return recipeList;
        }
        public List<RecipeDomain> GetRecipeList()
        {
            List<RecipeDomain> recipeList = new List<RecipeDomain>();
            try
            {
                var listRecipe = connectionModel.Recipes.ToList();
                foreach (var item in listRecipe)
                {
                    var recipe = new RecipeDomain();
                    recipe.idRecipe = item.IdRecipe;
                    recipe.user_idUser = item.UserIdUser;
                    recipe.recipeName = item.ReceipName;
                    recipe.imageRecipeURL = item.ImageRecipeUrl;
                    recipe.preparationTime = item.PreparationTime.ToString();
                    recipe.idMainIngredient = item.IdMainIngredient;
                    recipe.portion = item.Portion;
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
                    recipe.user_idUser = item.UserIdUser;
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

        public bool AddFavoriteRecipe(string idUser, string idRecipe)
        {
            bool flag = false;
            try
            {
                // Verificar si la receta ya está en la lista de favoritos del usuario
                var existingFavorite = connectionModel.Recipes
                    .Include(r => r.IdUserFavorites)
                    .FirstOrDefault(s => s.IdUserFavorites.Equals(idUser) && s.IdRecipe == idRecipe);

                if (existingFavorite == null)
                {
                    var recipe = connectionModel.Recipes.Where(r => r.IdRecipe.Equals(idRecipe)).FirstOrDefault();
                    var user = connectionModel.Users.Where(r => r.IdUser == idUser).FirstOrDefault();

                    if (recipe != null && user != null)
                    {
                        recipe.IdUserFavorites.Add(user);
                        flag = true;
                    }

                }
            }
            catch (Exception e)
            {
                throw new ExceptionBusiness("Error al agregar a recetas favoritas: " + e.Message);
            }
            return flag;
        }

        public bool DeleteFavoriteRecipe(string idUser, string idRecipe)
        {
            bool flag = false;
            try
            {
                // Verificar si la receta ya está en la lista de favoritos del usuario
                var existingFavorite = connectionModel.Recipes
                    .Include(r => r.IdUserFavorites)
                    .FirstOrDefault(s => s.IdUserFavorites.Equals(idUser) && s.IdRecipe == idRecipe);

                if (existingFavorite == null)
                {
                    var recipe = connectionModel.Recipes.Where(r => r.IdRecipe.Equals(idRecipe)).FirstOrDefault();
                    var user = connectionModel.Users.Where(r => r.IdUser == idUser).FirstOrDefault();

                    if (user != null && recipe != null)
                    {
                        recipe.IdUserFavorites.Remove(user);
                        connectionModel.SaveChanges();
                        flag = true;
                    }

                }
            }
            catch (Exception e)
            {
                throw new ExceptionBusiness("Error al eliminar de recetas favoritas: " + e.Message);
            }
            return flag;
        }

        public (int, string, string) PostRecipe(RecipeDomain newRecipe, CategoryDomain categoryDomain)
        {
            string errormsg = "";
            int changes = 0;
            string idRecipeNew = GenerateRandomID.GenerateID();

            try
            {
                string imagePath = "";
                string nameImage = GenerateRandomID.GenerateID();
                byte[] imageBytes = Convert.FromBase64String(newRecipe.imageBase);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(ms);

                    // Guarda la imagen en la carpeta wwwroot/images con un nombre único
                    imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Images");
                    string fileName = nameImage + ".jpg";
                    string fullPath = Path.Combine(imagePath, fileName);

                    //Asegúrate de que la carpeta exista, si no, créala
                    if (!Directory.Exists(imagePath))
                    {
                        Directory.CreateDirectory(imagePath);
                    }

                    // Guarda la imagen
                    image.Save(fullPath);
                    if(!File.Exists(fullPath))
                    {
                        return(500,"ads","das");
                    }


                }



                ///////////////////////////
                Category categoryModel = connectionModel.Categories.Find(categoryDomain.idCategory);
                if (categoryModel != null)
                {
                    Recipe recipeTemp = new Recipe
                    {
                        IdRecipe = idRecipeNew,
                        UserIdUser = newRecipe.user_idUser,
                        ReceipName = newRecipe.recipeName,
                        ImageRecipeUrl = "https://namnam-api2.azurewebsites.net/Images/" + nameImage + ".jpg",
                        PreparationTime = TimeOnly.Parse("00:00:00"),
                        IdMainIngredient = newRecipe.idMainIngredient,
                        Portion = newRecipe.portion,
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
                    recipeTemp.user_idUser = recipeModel.UserIdUser;
                    recipeTemp.imageRecipeURL = recipeModel.ImageRecipeUrl;
                    recipeTemp.preparationTime = recipeModel.PreparationTime.ToString();
                    recipeTemp.idMainIngredient = recipeModel.IdMainIngredient;
                    recipeTemp.portion = recipeModel.Portion;
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
                        step.idCookingInstruction = item.IdCookingInstruction;
                        step.instruction = item.Instruction;
                        step.recipeIdRecipe = item.RecipeIdRecipe;
                        step.step = item.Step;
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
                                recipe_Has_IngredientDomainTemp.recipe_idRecipe = amountTemp.RecipeIdRecipe;
                                recipe_Has_IngredientDomainTemp.ingredient_idIngredient = amountTemp.IngredientIdIngredient;
                                recipe_Has_IngredientDomainTemp.amount = amountTemp.Amount;
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
                    if (recipeModel.Portion != newRecipe.recipeDomain.portion)
                        recipeModel.Portion = newRecipe.recipeDomain.portion;
                    if (recipeModel.ReceipName != newRecipe.recipeDomain.recipeName)
                        recipeModel.ReceipName = newRecipe.recipeDomain.recipeName;
                    if (recipeModel.IdMainIngredient != newRecipe.recipeDomain.idMainIngredient)
                        recipeModel.IdMainIngredient = newRecipe.recipeDomain.idMainIngredient;
                    //edicion de categoria
                    foreach (var category in recipeModel.CategoryIdCategories.ToList())
                    {
                        recipeModel.CategoryIdCategories.Remove(category);
                    }
                    Category categoryModel = connectionModel.Categories.Find(newRecipe.category.idCategory);
                    recipeModel.CategoryIdCategories.Add(categoryModel);
                    //edicion de ingredientes
                    List<RecipeHasIngredient> list = new List<RecipeHasIngredient>();
                    foreach (var item in newRecipe.recipeHasIngredients)
                    {
                        RecipeHasIngredient recipeHasIngredient = new RecipeHasIngredient();
                        recipeHasIngredient.IngredientIdIngredient = item.ingredient_idIngredient;
                        recipeHasIngredient.RecipeIdRecipe = newRecipe.recipeDomain.idRecipe;
                        recipeHasIngredient.Amount = item.amount;
                        list.Add(recipeHasIngredient);
                    }
                    connectionModel.RecipeHasIngredients.RemoveRange(connectionModel.RecipeHasIngredients.Where(a => a.RecipeIdRecipe == newRecipe.recipeDomain.idRecipe));
                    connectionModel.RecipeHasIngredients.AddRange(list);
                    //edicion de los pasos
                    List<Cookinginstruction> instructionsTemp = new List<Cookinginstruction>();
                    foreach (var item in newRecipe.instructions)
                    {

                        Cookinginstruction itemBD = new Cookinginstruction
                        {
                            IdCookingInstruction = GenerateRandomID.GenerateID(),
                            Instruction = item.instruction,
                            Step = (int)item.step,
                            RecipeIdRecipe = newRecipe.recipeDomain.idRecipe
                        };
                        instructionsTemp.Add(itemBD);
                    }
                    connectionModel.Cookinginstructions.RemoveRange(connectionModel.Cookinginstructions.Where(a => a.RecipeIdRecipe == newRecipe.recipeDomain.idRecipe));
                    connectionModel.Cookinginstructions.AddRange(instructionsTemp);
                    connectionModel.SaveChanges();

                    var imageOld = recipeModel.ImageRecipeUrl;
                    //Guardar nueva imagen
                    string nameImage = GenerateRandomID.GenerateID();
                    byte[] imageBytes = Convert.FromBase64String(newRecipe.recipeDomain.imageBase);
                    string fullPath = "";
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        //DeleteImage
                        if (File.Exists(imageOld))
                        {
                            // Elimina la imagen existente
                            File.Delete(imageOld);
                        }
                        Image image = Image.FromStream(ms);
                        // Guarda la imagen en la carpeta wwwroot/images con un nombre único
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Images");
                        string fileName = nameImage + ".jpg";
                        fullPath = Path.Combine(imagePath, fileName);

                        //Asegúrate de que la carpeta exista, si no, créala
                        if (!Directory.Exists(imagePath))
                        {
                            Directory.CreateDirectory(imagePath);
                        }

                        // Guarda la imagen
                        image.Save(fullPath);
                    }
                    recipeModel.ImageRecipeUrl = "https://namnam-api2.azurewebsites.net/Image/" + nameImage + ".jpg";
                    connectionModel.SaveChanges();
                    result = true;
                }
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
