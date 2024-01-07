using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models
{
    public class User 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        [Required]
        [EmailAddress (ErrorMessage = "Invalid email")]
        public string? email { get; set; }
        [Required]
        public string? password { get; set; }
        public string? avatar { get; set; }
        public string? birthday { get; set; }
        public string? fullname { get; set; }
        public string? gender { get; set; }
    }
}
