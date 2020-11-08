using System;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskManager.Models
{
    public class Task : MongoBaseModel
    {

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("Body")]
        public string Body { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("Done")]
        [BsonDefaultValue("false")]
        public bool Done {get; set;}

        


    }
}