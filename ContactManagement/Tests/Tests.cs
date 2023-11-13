using ContactManagement.Contact.Models;
using ContactManagement.Contact.Services;
using ContactManagement.MongoRepository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ContactManagement.Tests
{
    public class ContactServiceTests : IAsyncLifetime
    {
        private IMongoDatabase _database;
        private IMongoCollection<Contacts> _contactCollection;
        private IMongoDbServices<Contacts> _contact;
        private readonly IMongoDbSettings _mongoDbSettings;

        public ContactServiceTests(IMongoDatabase database, IMongoCollection<Contacts> contactCollection, IMongoDbServices<Contacts> contact)
        {
            _database = database;
            _contactCollection = contactCollection;
            _contact = contact;
            _mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = "mongodb+srv://agirbasegecan:1234@cluster0.rwrvo25.mongodb.net/?retryWrites=true&w=majority",
                DatabaseName = "TestContactList" 
            };
        }

        public async Task InitializeAsync()
        {
            var client = new MongoClient(_mongoDbSettings.ConnectionString);
            _database = client.GetDatabase(_mongoDbSettings.DatabaseName);
            _contactCollection = _database.GetCollection<Contacts>("contacts");
            
            await _contactCollection.DeleteManyAsync(FilterDefinition<Contacts>.Empty);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task GetAllContacts_ShouldReturnListOfContacts()
        {
            var contactService = new ContactService(_mongoDbSettings, _contactCollection, _contact);
            
            await contactService.Create(new Contacts
            {
                Id = "1",
                Name = "John",
                Surname = "Doe",
                Firm = "ABC Inc."
            });

            var contacts = await contactService.GetAllContacts();
            
            Assert.NotNull(contacts);
            Assert.IsType<List<Contacts>>(contacts);
            Assert.Equal(1, contacts.Count);
            
            await _contactCollection.DeleteManyAsync(FilterDefinition<Contacts>.Empty);
        }
    }
}
