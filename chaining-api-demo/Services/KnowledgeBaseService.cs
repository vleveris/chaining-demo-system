using ChainingAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChainingAPI.Services;

public class KnowledgeBaseService
{
    private readonly IMongoCollection<KnowledgeBase> _entitiesCollection;

    public KnowledgeBaseService(
        IOptions<FactsRulesDatabaseSettings> factsRulesDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            factsRulesDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            factsRulesDatabaseSettings.Value.DatabaseName);
        var options = new CreateIndexOptions() { Unique = true };
        var field = new StringFieldDefinition<KnowledgeBase>("Name");
        var indexDefinition = new IndexKeysDefinitionBuilder<KnowledgeBase>().Ascending(field);
        var indexModel = new CreateIndexModel<KnowledgeBase>(indexDefinition, options);
        _entitiesCollection = mongoDatabase.GetCollection<KnowledgeBase>(
            factsRulesDatabaseSettings.Value.FactsRulesCollectionName);
        _entitiesCollection.Indexes.CreateOne(indexModel);
    }

    public async Task<KnowledgeBase?> GetByIdAsync(string id) =>
        await _entitiesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<KnowledgeBase?> GetByNameAsync(string name) =>
        await _entitiesCollection.Find(x => x.Name == name).FirstOrDefaultAsync();

    public async Task CreateAsync(KnowledgeBase newEntity) =>
        await _entitiesCollection.InsertOneAsync(newEntity);

    public async Task UpdateAsync(string id, KnowledgeBase updatedEntity) =>
        await _entitiesCollection.ReplaceOneAsync(x => x.Id == id, updatedEntity);

    public async Task RemoveAsync(string id) =>
        await _entitiesCollection.DeleteOneAsync(x => x.Id == id);
}