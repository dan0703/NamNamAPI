﻿using System;
using System.Collections.Generic;

namespace NamNamAPI.Models;

public partial class Recipe
{
    public string IdRecipe { get; set; } = null!;

    public string UserIdUser { get; set; } = null!;

    public string ReceipName { get; set; } = null!;

    public string ImageRecipeUrl { get; set; } = null!;

    public TimeOnly PreparationTime { get; set; }

    public string IdMainIngredient { get; set; } = null!;

    public virtual ICollection<Cookinginstruction> Cookinginstructions { get; set; } = new List<Cookinginstruction>();

    public virtual Ingredient IdMainIngredientNavigation { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User UserIdUserNavigation { get; set; } = null!;

    public virtual ICollection<Category> CategoryIdCategories { get; set; } = new List<Category>();

    public virtual ICollection<Ingredient> IngredientIdIngredients { get; set; } = new List<Ingredient>();

    public virtual ICollection<User> UserIdUsers { get; set; } = new List<User>();

    public virtual ICollection<User> UserIdUsersNavigation { get; set; } = new List<User>();
}