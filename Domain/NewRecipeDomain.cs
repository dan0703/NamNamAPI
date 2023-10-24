using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace NamNamAPI.Domain;

public class NewRecipeDomain{
    public RecipeDomain recipeDomain {get; set;}
    public List<CookinginstructionDomain> instructions {get
    ;set;}
}