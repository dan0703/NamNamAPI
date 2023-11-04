namespace NamNamAPI.Domain;

public class NutritionalDataDomain
{
    public string IdNutritionalData { get; set; } 

    public int? Calories { get; set; }

    public int? Fat { get; set; }

    public int? Protein { get; set; }

    public int? Carbohydrates { get; set; }

    public string IngredientIdIngredient { get; set; }
}