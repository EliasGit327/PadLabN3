using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Persistence.Configuration;
using Persistence.Models;

namespace Persistence.Repositories
{
    public class MessageRepository
    {
        private readonly IMongoCollection<Message> _messages;

        public MessageRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _messages = database.GetCollection<Message>(settings.CollectionName);
        }

        public Task<List<Message>> Get() => _messages.Find(message => true).ToListAsync();

        public Task<Message> Get(string id) => _messages.Find(message => message.Id == id).FirstOrDefaultAsync();

        public async Task<Message> Create(Message message)
        {
            await _messages.InsertOneAsync(message);
            return message;
        }

        public Task Update(string id, Message bookIn) => _messages.ReplaceOneAsync(message => message.Id == id, bookIn);

        public Task Remove(Message message) => Remove(message.Id);

        public Task Remove(string id) => _messages.DeleteOneAsync(message => message.Id == id);
    }
}