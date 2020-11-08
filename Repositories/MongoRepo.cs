using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using TaskManager.Models;

namespace TaskManager.Repositories
{
    public class MongoRepo<T> where T : MongoBaseModel
    {
        private readonly IMongoCollection<T> mongoCollection;

        public MongoRepo(string mongoDbConnectionString, string dbName, string collectionName)
        {

            var client = new MongoClient(mongoDbConnectionString);
            var database = client.GetDatabase(dbName);
            mongoCollection = database.GetCollection<T>(collectionName);
        }

        public virtual List<T> GetAll()
        {
            return mongoCollection.Find(m => true).ToList();
        }

        public virtual T GetById(string id)
        {

            return mongoCollection.Find<T>(m => m.Id == id).FirstOrDefault();
        }

        public virtual T Create(T model)
        {
            mongoCollection.InsertOne(model);
            return model;
        }

        public virtual void Update(string id, T model)
        {
            mongoCollection.ReplaceOne(m => m.Id == id, model);
        }

        public virtual void Remove(T model)
        {
            mongoCollection.DeleteOne(m => m.Id == model.Id);
        }

        public virtual void Remove(string id)
        {
            mongoCollection.DeleteOne(m => m.Id == id);
        }

        

        
    }
}