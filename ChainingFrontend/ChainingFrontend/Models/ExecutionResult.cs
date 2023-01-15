namespace ChainingFrontend.Models
{
    public class ExecutionResult
    {
            public bool GoalAchieved { get; set; }
            public List<int> Path { get; set; }
            public string Trace { get; set; }

}
}
