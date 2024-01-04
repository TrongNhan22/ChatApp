using ChatApp.Models;
using MongoDB.Driver;

namespace ChatApp.Data
{
    public class MongoDBSetting
    {
        public string? ConnectionURI { get; set; }
        public string? DatabaseName { get; set; }
        public string? userCollectionName { get; set; }
        public IMongoCollection<User>? userCollection { get; set; }

        MongoDBSetting() {
            MongoClient client = new MongoClient(ConnectionURI);
            IMongoDatabase database = client.GetDatabase(DatabaseName);

            userCollection = database.GetCollection<User>(userCollectionName);
        }
    }
}
