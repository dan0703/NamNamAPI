using System;
using System.Collections.Generic;

namespace NamNamAPI.Models;

public partial class RecipeHasIngredient
{
    public string IngredientIdIngredient { get; set; } = null!;

    public string RecipeIdRecipe { get; set; } = null!;

    public int? Amount { get; set; }

    public virtual Ingredient IngredientIdIngredientNavigation { get; set; } = null!;

    public virtual Recipe RecipeIdRecipeNavigation { get; set; } = null!;
}
