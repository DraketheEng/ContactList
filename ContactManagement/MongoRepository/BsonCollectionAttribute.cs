// BsonCollectionAttribute, MongoDB koleksiyonunu temsil etmek üzere kullanılan özel bir özelliktir.
// Bu özellik, bir sınıfın üzerine eklendiğinde, bu sınıfın MongoDB koleksiyon adını belirlemek için kullanılır.
// Attribute sınıfından türetilmiştir, bu nedenle sınıflar üzerine eklenir ve adı verilen bir özellik içerir.

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute : Attribute
{
    // CollectionName, MongoDB koleksiyon adını içeren bir özelliktir.
    public string? CollectionName { get; }

    // BsonCollectionAttribute sınıfının yapıcı metodu.
    // Bu metod, koleksiyon adını parametre olarak alır ve sınıfa atanır.
    public BsonCollectionAttribute(string? collectionName)
    {
        CollectionName = collectionName;
    }
}