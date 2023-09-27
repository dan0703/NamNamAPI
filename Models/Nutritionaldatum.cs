using System;
using System.Collections.Generic;

namespace NamNamAPI.Models;

public partial class Nutritionaldatum
{
    public string IdNutritionalData { get; set; } = null!;

    public int? Calories { get; set; }

    public int? Fat { get; set; }

    public int? Protein { get; set; }

    public int? Carbohydrates { get; set; }

    public string IngredientIdIngredient { get; set; } = null!;

    public virtual Ingredient IngredientIdIngredientNavigation { get; set; } = null!;
}
