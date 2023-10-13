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
                    IngredientDomain ingredient = new IngredientDomain{
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
    }
}
