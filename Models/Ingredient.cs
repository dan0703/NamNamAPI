using System;
using System.Collections.Generic;

namespace NamNamAPI.Models;

public partial class Ingredient
{
    public string IdIngredient { get; set; } = null!;

    public string IngredientName { get; set; } = null!;

    public string Measure { get; set; } = null!;

    public virtual ICollection<Nutritionaldatum> Nutritionaldata { get; set; } = new List<Nutritionaldatum>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<Recipe> RecipeIdRecipes { get; set; } = new List<Recipe>();
}
