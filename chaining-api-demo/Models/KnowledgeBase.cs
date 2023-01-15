using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChainingAPI.Models;

public class KnowledgeBase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; } = null!;
    public List<string>? Facts { get; set; }
    public List<Rule>? Rules { get; set; }
}