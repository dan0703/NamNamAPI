using NamNamAPI.Models;

namespace NamNamAPI.Domain;

public class RecipeDomain{
    public string? idRecipe {get;set;}
    public string? User_idUser {get;set;}
    public string? recipeName {get;set;}
    public string? imageRecipeURL {get;set;}
    public string? preparationTime { get;set;}
    public string? idMainIngredient { get;set;}
    public int? Portion { get; set; }
    public byte[]? ImageBytes {get;set;}
}