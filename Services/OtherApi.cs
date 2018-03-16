using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using recipeautomaticcreateservice.Model;
using recipeautomaticcreateservice.Services.Interfaces;

namespace recipeautomaticcreateservice.Services
{
    public class OtherApi : IOtherApi
    {
        private readonly IConfiguration _configuration;
        HttpClient client = new HttpClient();
        public OtherApi (IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Tag>> GetTags()
        {            
            List<Tag> tagList = new List<Tag>();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["tagServiceEndpoint"]);
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    var  returnApi=  JObject.Parse(await client.GetStringAsync(url));
                    foreach(var tagJObject in returnApi["values"])
                    {
                        tagList.Add(tagJObject.ToObject<Tag>());
                    }
                    return tagList;
                case HttpStatusCode.NotFound:
                    return null;
                case HttpStatusCode.InternalServerError:
                    return null;
            }
            return null;
        }

        public async Task<dynamic> GetPhaseLinha()
        {            
            
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var builder = new UriBuilder(_configuration["phaseLinhaEndpoint"]);
            string url = builder.ToString();
            var result = await client.GetAsync(url);
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    var phaseLinha = JsonConvert.DeserializeObject<dynamic>(await client.GetStringAsync(url));                    
                    return phaseLinha;
                case HttpStatusCode.NotFound:
                    return null;
                case HttpStatusCode.InternalServerError:
                    return null;
            }
            return null;
        }

        public async Task<bool> PostPhaseInRecipe(dynamic json,int recipeId)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(json).ToString(), Encoding.UTF8,"application/json");
            var builder = new UriBuilder(_configuration["PostPhaseInRecipeServiceEndpoint"] + recipeId.ToString());
            string url = builder.ToString();
            var result = await client.PostAsync(url,contentPost);
            
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:                    
                    return true;
                case HttpStatusCode.Created:
                    return true;
                case HttpStatusCode.InternalServerError:
                    return false;
            }
            return false;

        }

        public async Task<Phase> PostPhase(dynamic json)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(json).ToString(), Encoding.UTF8,"application/json");
            var builder = new UriBuilder(_configuration["PostPhaseServiceEndpoint"]);
            string url = builder.ToString();
            var result = await client.PostAsync(url,contentPost);

            switch (result.StatusCode)
            {
                case HttpStatusCode.Created:
                    var phase = JsonConvert.DeserializeObject<Phase>(await result.Content.ReadAsStringAsync()); 
                    return phase;
                case HttpStatusCode.InternalServerError:
                    return null;
            }
            return null;

        }

         public async Task<bool> PostParameterInPhase(dynamic json,int phaseId)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(json).ToString(), Encoding.UTF8,"application/json");
            var builder = new UriBuilder(_configuration["PostParameterInPhaseServiceEndpoint"] + phaseId);
            string url = builder.ToString();
            var result = await client.PostAsync(url,contentPost);

            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true; 
                case HttpStatusCode.Created:                    
                    return true;
                case HttpStatusCode.InternalServerError:
                    return false;
            }
            return false;

        }
    }
}