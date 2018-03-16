using System.Collections.Generic;
using System.Threading.Tasks;
using recipeautomaticcreateservice.Model;

namespace recipeautomaticcreateservice.Services.Interfaces
{
    public interface IOtherApi
    {
         Task<List<Tag>> GetTags();
         Task<dynamic> GetPhaseLinha();
         Task<bool> PostPhaseInRecipe(dynamic json,int recipeId);
         Task<Phase> PostPhase(dynamic json);
         Task<bool> PostParameterInPhase(dynamic json,int phaseId);
    }
}