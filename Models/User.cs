using System;
using System.Collections.Generic;

namespace NamNamAPI.Models;

public partial class User
{
    public string IdUser { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Preference> Preferences { get; set; } = new List<Preference>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Category> CategoryIdCategories { get; set; } = new List<Category>();

    public virtual ICollection<Recipe> RecipeIdRecipes { get; set; } = new List<Recipe>();
}
