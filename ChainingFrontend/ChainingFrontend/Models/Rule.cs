namespace ChainingFrontend.Models
{
    public class Rule
    {
        public int Number { get; set; }
        public string Consequent { get; set; }
        public List<string> Antecedent { get; set; }
        public override string ToString()
        {
if (Antecedent == null || Antecedent.Count <1)
            {
                return null;
            }
            return string.Join(", ", Antecedent) + " -> " + Consequent;
        }
    }
}