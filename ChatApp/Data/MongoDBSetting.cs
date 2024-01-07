namespace ChatApp.Data
{
    public class MongoDBSetting
    {
        public string? ConnectionURI { get; set; }
        public string? DatabaseName { get; set; }
        public string MessageCollectionName { get; set; } = null!;
    }
}
