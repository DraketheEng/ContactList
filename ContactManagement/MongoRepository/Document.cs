// Document sınıfı, MongoDB'deki belgeleri temsil etmek üzere kullanılan soyut bir sınıftır.
// Bu sınıf, tüm belgelerin ortak özelliklerini içerir.

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContactManagement.MongoRepository
{
    // BsonRepresentation ve BsonElement özelliklerini içeren bu soyut sınıf, MongoDB'ye kaydedilecek belgelerin genel şablonunu sağlar.
    public abstract class Document : IDocument
    {
        [BsonElement("id")]
        [BsonRepresentation(BsonType.String)]
        public string? Id { get; set; }
        
        public DateTime? CreateDate { get; set; }
        
        public DateTime? UpdateDate { get; set; }
    }
}