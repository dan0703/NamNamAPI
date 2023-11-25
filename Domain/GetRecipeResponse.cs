using NamNamAPI.Models;

namespace NamNamAPI.Domain;

public class GetRecipeResponse{
    public RecipeDomain recipe {get; set;}
    public CategoryDomain category {get; set;}
    public List<CookinginstructionDomain> stepList {get;set;}
    public List<IngredientDomain> ingredientList {get;set;}
    public List<Recipe_has_IngredientDomain> ingredientAmounList {get;set;}
    public List<NutritionalDataDomain> nutritionalDataList {get; set;}
}