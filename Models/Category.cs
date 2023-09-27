using System;
using System.Collections.Generic;

namespace NamNamAPI.Models;

public partial class Category
{
    public string IdCategory { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Preference> Preferences { get; set; } = new List<Preference>();

    public virtual ICollection<Recipe> RecipeIdRecipes { get; set; } = new List<Recipe>();

    public virtual ICollection<User> UserIdUsers { get; set; } = new List<User>();
}
