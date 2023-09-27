using System;
using System.Collections.Generic;

namespace NamNamAPI.Models;

public partial class Cookinginstruction
{
    public string IdCookingInstruction { get; set; } = null!;

    public string Instruction { get; set; } = null!;

    public int Step { get; set; }

    public string RecipeIdRecipe { get; set; } = null!;

    public virtual Recipe RecipeIdRecipeNavigation { get; set; } = null!;
}
