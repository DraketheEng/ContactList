using System.Linq.Expressions;

namespace ContactManagement.MongoRepository;

public interface IMongoDbServices<TDocument> where TDocument : IDocument
{
    Task CreateAsync(TDocument document);

    Task ReplaceOneAsync(TDocument document);

    Task DeleteByIdAsync(string id);

    Task<TDocument> GetById(string id);

    Task<List<TDocument>> GetAllAsync();

    Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression);
}