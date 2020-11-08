using TaskManager.Models;

namespace TaskManager.Repositories
{
    public class TasksRepository : MongoRepo<Task>
    {
        public TasksRepository(string mongoDbConnectionString, string dbName, string collectionName) : base(mongoDbConnectionString, dbName, collectionName)
        {
        }
    }
}