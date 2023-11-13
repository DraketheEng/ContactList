namespace ContactManagement.MongoRepository
{
    // MongoDB bağlantı ayarlarını içeren sınıf.
    public class MongoDbSettings : IMongoDbSettings
    {
        // Database adını alır veya ayarlar.
        public string DatabaseName { get; set; }
        
        // Database bağlantı dizesini alır veya ayarlar.
        public string ConnectionString { get; set; }
    }
}