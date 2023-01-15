using System.Text.Json;
using System.Text.Json.Serialization;
namespace ChainingAPI.Models
{
    public class Rule
    {
        public int Number { get; set; }
        public string Consequent { get; set; }
        public List<string> Antecedent { get; set; }
        [JsonIgnore]
        public bool Flag1 { get; set; } = false;
        [JsonIgnore]
        public bool Flag2 { get; set; } = false;

        public override string ToString()
        {
            if (Antecedent == null || Antecedent.Count < 1)
            {
                return null;
            }
            return string.Join(", ", Antecedent) + " -> " + Consequent;
        }
    }
}