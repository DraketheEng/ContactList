using ContactList.ContactList.Reporting.Services;
using ContactManagement.Contact.Models;
using ContactManagement.Contact.Services;
using ContactManagement.MongoRepository;
using ContactManagement.RabbitMqService;
using ContactManagement.ReportingSystem.Models;
using ContactManagement.ReportingSystem.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMQ"));


var mongoClient = new MongoClient(builder.Configuration.GetSection("MongoDbSettings:ConnectionString").Value);
var database = mongoClient.GetDatabase(builder.Configuration.GetSection("MongoDbSettings:DatabaseName").Value);

void AddScopedCollection<TDocument>(IServiceCollection services)
{
    services.AddScoped<IMongoCollection<TDocument>>(sp =>
    {
        var collectionName = GetCollectionName(typeof(TDocument));

        string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute), true).FirstOrDefault()!)?.CollectionName!;
        }

        return database.GetCollection<TDocument>(collectionName);
    });
}

var collectionHelper = new MongoDbServices<IDocument>.CollectionHelper();
builder.Services.AddSingleton(collectionHelper);

AddScopedCollection<Contacts>(builder.Services);
AddScopedCollection<Report>(builder.Services);
AddScopedCollection<ReportStatus>(builder.Services);

builder.Services.AddSingleton<IMongoDbSettings>(serviceProvider =>
    serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

builder.Services.AddScoped(typeof(IMongoDbServices<>), typeof(MongoDbServices<>));
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IReportingService, ReportingService>();
builder.Services.AddScoped<IMongoDbServices<ReportStatus>, MongoDbServices<ReportStatus>>();
builder.Services.AddScoped<RabbitMqService>();
builder.Services.AddControllers();

builder.Services.AddLogging(configure =>
{
    configure.AddConsole(); // Bu satır, konsol logger'ını etkinleştirir
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();