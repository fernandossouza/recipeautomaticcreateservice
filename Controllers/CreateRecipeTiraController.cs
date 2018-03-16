using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using recipeautomaticcreateservice.Services.Interfaces;
using recipeautomaticcreateservice.Model;

namespace recipeautomaticcreateservice.Controllers
{
    [Route("api/[controller]")]
    public class CreateRecipeTiraController : Controller
    {
        private readonly IOpTiraService _recipeTiraService;
        public CreateRecipeTiraController(IOpTiraService recipeTiraService)
        {
            _recipeTiraService = recipeTiraService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Recipe recipe)
        {
            var (returnService,stringErro) = await _recipeTiraService.CreateAutomaticRecipe(recipe);
            if(!returnService)
                return StatusCode(500, stringErro);

            return Ok();

        }
        
    }
}