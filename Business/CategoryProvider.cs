namespace NamNamAPI.Business
{
    using System.Globalization;
    using NamNamAPI.Domain;
    using NamNamAPI.Models;

    public class CategoryProvider
    {
        private NamnamContext connectionModel;

        public CategoryProvider(NamnamContext _connectionModel)
        {
            string Culture = "es-MX";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Culture);
            connectionModel = _connectionModel;
        }

        public (int, List<CategoryDomain>,string) getCategories()
        {
            int code = 200;
            List<CategoryDomain> categoryList = new List<CategoryDomain>();
            string report = "";
            try
            {
                var listTemp = connectionModel.Categories.ToList();
                foreach (var item in listTemp)
                {
                    CategoryDomain category = new CategoryDomain{
                        idCategory = item.IdCategory,
                        categoryName = item.CategoryName
                    };
                    categoryList.Add(category);
                }
            }
            catch (Exception e)
            {
                code = 500;
                report = e.Message;
            }
            return (code, categoryList,report);
        }

        // public int SetRecipeHasCategory(CategoryDomain category, int idRecipe){//pendiente
        //     int changes = 0;
        //         RecipeHasCategory recipeHasIngredient = new RecipehasCategory{

        //         }
        //     return changes;
        // }
    }
}
