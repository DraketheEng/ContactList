using MongoDB.Bson.Serialization.Attributes;

namespace ContactManagement.MongoRepository;

public interface IDocument
{
    [BsonId]
    [BsonElement("id")]
    string? Id { get; set; }

    DateTime? CreateDate { get; set; }

    DateTime? UpdateDate { get; set; }
}