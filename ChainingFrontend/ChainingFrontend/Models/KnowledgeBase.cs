namespace ChainingFrontend.Models
{
    public class KnowledgeBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Facts { get; set; }
        public List<Rule> Rules { get; set; }
    }
}
