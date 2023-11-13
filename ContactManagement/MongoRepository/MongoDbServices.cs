using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ContactManagement.MongoRepository
{
    public class MongoDbServices<TDocument> : IMongoDbServices<TDocument> where TDocument : IDocument
    {
        protected readonly IMongoCollection<TDocument> Collection;

        public MongoDbServices(IMongoDbSettings settings, IMongoCollection<TDocument> contactCollection)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            string collectionName = GetCollectionName(typeof(TDocument));
            Collection = database.GetCollection<TDocument>(collectionName);
        }
        
        public class CollectionHelper
        {
            public string GetCollectionName(Type documentType)
            {
                return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute), true).FirstOrDefault()!)?.CollectionName!;
            }
        }

        // Belge türüne özgü koleksiyon adını almak için kullanılan özel bir yardımcı metod.
        private string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute), true).FirstOrDefault()!)?.CollectionName!;
        }

        // Belge eklemeyi sağlar. Belge kimliği yoksa oluşturur ve oluşturulma tarihini ayarlar.
        public virtual Task CreateAsync(TDocument document)
        {
            if (string.IsNullOrEmpty(document.Id))
            {
                document.Id = Guid.NewGuid().ToString();
            }

            document.CreateDate = DateTime.Now;

            return Task.Run(() => Collection.InsertOneAsync(document));
        }

        // Belirtilen filtreleme ifadesine göre belgeleri getirir.
        public virtual async Task<IEnumerable<TDocument>> FilterByAsync(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            var cursor = await Collection.FindAsync(filterExpression);
            return await cursor.ToListAsync();
        }


        // Tüm belgeleri asenkron olarak getirir.
        public async Task<List<TDocument>> GetAllAsync()
        {
            var cursor = await Collection.FindAsync(_ => true);
            return await cursor.ToListAsync();
        }

        // ID'ye göre belge getirir.
        public virtual Task<TDocument> GetById(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
                return Collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        // Belge ID'ye göre belge siler.
        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
                Collection.FindOneAndDeleteAsync(filter);
            });
        }

        // Belgeyi günceller, Güncelleme tarihini ayarlar.
        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            document.UpdateDate = DateTime.Now;

            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await Collection.FindOneAndReplaceAsync(filter, document);
        }
    }
}