namespace ChainingAPI.Models;

public class FactsRulesDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string FactsRulesCollectionName { get; set; } = null!;
}