using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson;

namespace ChatApp.Models
{
    public class ConnectionDatabase
    {
        private IMongoCollection<BsonDocument> _collection;
        public ConnectionDatabase()
        {
            var connectionString = "mongodb+srv://se310:1@se310.0vffe0y.mongodb.net/?retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            var client = new MongoClient(settings);
            var database = client.GetDatabase("SE310");
            _collection = database.GetCollection<BsonDocument>("user");
        }
        public BsonDocument GetUser(string userName)
        {
            var userId = ObjectId.Parse(userName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", userId);
            return _collection.Find(filter).FirstOrDefault();
        }
    }
}
