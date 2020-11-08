using System;

namespace TaskManager.DTOs
{
    public class TaskPutDTO
    {
        public string Title { get; set; }


        public string Body { get; set; }


        public DateTime Date { get; set; }

        public bool Done {get; set;}
    }
}