using System;
using System.Collections.Generic;

namespace NamNamAPI.Models;

public partial class Review
{
    public string IdReview { get; set; } = null!;

    public string? Review1 { get; set; }

    public int? Rate { get; set; }

    public string UserIdUser { get; set; } = null!;

    public string RecipeIdRecipe { get; set; } = null!;

    public virtual Recipe RecipeIdRecipeNavigation { get; set; } = null!;

    public virtual User UserIdUserNavigation { get; set; } = null!;
}
