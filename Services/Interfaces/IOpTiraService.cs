using System.Threading.Tasks;
using recipeautomaticcreateservice.Model;

namespace recipeautomaticcreateservice.Services.Interfaces
{
    public interface IOpTiraService
    {
         Task<(bool,string)> CreateAutomaticRecipe(Recipe recipe);
    }
}