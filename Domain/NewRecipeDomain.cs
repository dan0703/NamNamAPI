using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using NamNamAPI.Models;

namespace NamNamAPI.Domain;

public class NewRecipeDomain{
    public RecipeDomain recipeDomain {get; set;}
    public List<CookinginstructionDomain> instructions {get; set;}
    public Category category {get; set;}
    public  Recipe_has_IngredientDomain recipeHasIngredients {get;set;}
}