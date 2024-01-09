using ChatApp.Models;
using MongoDB.Driver;

namespace ChatApp.Data
{
    public class MongoDBSetting
    {
        public string? ConnectionURI { get; set; }
        public string? DatabaseName { get; set; }
        public string? userCollectionName { get; set; }
        public string? relationshipCollectionName { get; set; }
        public string? friendRequestCollectionName { get; set; }
        public string MessageCollectionName { get; set; } = null!;
        public string RelationshipCollectionName { get; set; } = null!;
    }
}
