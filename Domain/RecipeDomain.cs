using NamNamAPI.Models;

namespace NamNamAPI.Domain;

public class RecipeDomain{
    public string? idRecipe {get;set;}
    public string? user_idUser {get;set;}
    public string? recipeName {get;set;}
    public string? imageRecipeURL {get;set;}
    public string? preparationTime { get;set;}
    public string? idMainIngredient { get;set;}
    public int? portion { get; set; }
    public string? imageBase {get;set;}
}