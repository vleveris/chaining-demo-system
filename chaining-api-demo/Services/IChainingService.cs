using ChainingAPI.Models;
namespace ChainingAPI.Services
{
    public interface IChainingService
    {
        public ExecutionResult Execute(KnowledgeBase inferenceEntity, string goal, bool trace);

    }
}
