using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using recipeautomaticcreateservice.Model;
using recipeautomaticcreateservice.Services.Interfaces;

namespace recipeautomaticcreateservice.Services
{
    public class OpTiraService : IOpTiraService
    {
        private readonly IConfiguration _configuration;
        private readonly IOtherApi _otherApiService;
        public OpTiraService(IConfiguration configuration, IOtherApi otherApiService)
        {
            _configuration = configuration;
            _otherApiService = otherApiService;
        }

        public async Task<(bool,string)> CreateAutomaticRecipe(Recipe recipe)
        {
            try
            {
                // faz gets das tags
                var tagList = await _otherApiService.GetTags();

                if(tagList == null || tagList.Count <1)
                    return(false,"Não houve retorno das tags de parametros");
                

                // faz get da phase linha
                var phaseLinha = await _otherApiService.GetPhaseLinha();

                if(phaseLinha == null)
                    return(false,"Não houve retorno da phase com parametros da linha");

                // cria uma phase com parametros especificos da receita
                var (returnParameter,stringErro) = await CreatePhaseAndParameter(recipe,tagList);
                if(!returnParameter)
                    return(returnParameter,stringErro);

                // adiciona phase linha na receita
                if(! await _otherApiService.PostPhaseInRecipe(phaseLinha,recipe.recipeId))
                    return(false,"Erro ao tentar adiconar a phase linha na receita");

                return (true,string.Empty);
            
            }
            catch(Exception ex)
            {
                return (false,ex.Message);
            }
        }

        private async Task<(bool,string)> CreatePhaseAndParameter(Recipe recipe,List<Tag> tagList)
        {
            Phase phase = new Phase();

            phase.phaseCode = recipe.recipeCode;
            phase.phaseName = "Prod Tira "+ recipe.recipeCode;

            phase = await _otherApiService.PostPhase(phase);

            if(phase == null)
                return (false,"Erro para cria a phase de parametros para a receita");

            if(! await _otherApiService.PostPhaseInRecipe(phase,recipe.recipeId))
                    return(false,"Erro ao tentar adiconar a phase linha na receita");

            foreach(var tag in tagList)
            {
                PhaseParameter phaseParameter = new PhaseParameter();

                phaseParameter.tagId = tag.tagId;
                phaseParameter.setupValue = "0";

                if(!await _otherApiService.PostParameterInPhase(phaseParameter,phase.phaseId))
                    return (false,"Erro ao tentar adicionar o parametro a phase");

            }

            return (true,string.Empty);
        }

        
        
    }
}