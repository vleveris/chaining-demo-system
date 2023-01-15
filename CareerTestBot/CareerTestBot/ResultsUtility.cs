using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
namespace CareerTestBot
{
    public class ResultsUtility
    {
        private const string _apiURI = "https://localhost:7071/api";
        private const string _careerKnowledgeBaseName = "CAREER";
        private bool _factsChanged = false;
        public async Task<bool> SetFacts(List<string> facts)
        {
            HttpClient client = new();
            try
            {
                var response = await client.PutAsJsonAsync($"{_apiURI}/KnowledgeBase/facts/{_careerKnowledgeBaseName}", facts);
                response.EnsureSuccessStatusCode();
                _factsChanged = true;
                return true;
                            }
            catch (Exception)
            {
                _factsChanged = false;
                return false;
            }

        }

        public async Task<bool> InferGoal(string goal)
        {
            if (!_factsChanged)
            {
                return false;
            }
            HttpClient client = new();
            try
            {
                var response = Convert.ToBoolean(await client.GetStringAsync($"{_apiURI}/Chaining/execute-only/{_careerKnowledgeBaseName}?goal={goal}&inferenceMethod=ForwardChaining"));
                return response;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
