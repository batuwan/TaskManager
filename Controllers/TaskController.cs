using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Repositories;

namespace TaskManager.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {

        private readonly TasksRepository tasksRepository;

        public TaskController(TasksRepository tasksRepository)
        {
            this.tasksRepository = tasksRepository;
        }

        [HttpGet]
        public ActionResult GetAllList()
        {
            return Ok(this.tasksRepository.GetAll());
        }

        [HttpGet("{id}", Name = "GetTask")]
        public ActionResult GetTask(string id)
        {
            var model = this.tasksRepository.GetById(id);
            if (model == null)
            {
                return NotFound();
            }
            return Ok(model);
        }

        [HttpPost]
        public ActionResult AddTask(TaskPostDTO taskDTO)
        {
            Task task = new Task();
            task.Title = taskDTO.Title;
            task.Body = taskDTO.Body;
            task.Date = taskDTO.Date;
            this.tasksRepository.Create(task);

            return CreatedAtRoute("GetTask", new { id = task.Id.ToString() }, task);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateTask(string id, TaskPutDTO taskDTO)
        {
            var existModel = this.tasksRepository.GetById(id);
            if (existModel == null)
            {
                return NotFound();
            }
            existModel.Title = taskDTO.Title;
            existModel.Body = taskDTO.Body;
            existModel.Date = taskDTO.Date;
            existModel.Done = taskDTO.Done;
            this.tasksRepository.Update(id, existModel);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTask(string id)
        {
            var existModel = this.tasksRepository.GetById(id);
            if (existModel == null)
            {
                return NotFound();
            }
            this.tasksRepository.Remove(id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PatchTask(string id, [FromBody] JsonPatchDocument<Task> patchEntity)
        {
            var entity = tasksRepository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            patchEntity.ApplyTo(entity, ModelState);
            tasksRepository.Update(id, entity);
            return Ok(entity);
        }

        [HttpGet("weekly")]
        public ActionResult GetThisWeek()
        {

            List<Task> allTasks = tasksRepository.GetAll();
            List<Task> weeklyTasks = new List<Task>();

            for (var i = 0; i < allTasks.Count; i++)
            {
                if(DateInsideOneWeek(DateTime.Today, allTasks[i].Date)){
                    weeklyTasks.Add(allTasks[i]);
                }
            }

            return Ok(weeklyTasks);
        }

        [HttpGet("weekly/{dateTime}")]
        public ActionResult GetWeek(DateTime dateTime)
        {

            List<Task> allTasks = tasksRepository.GetAll();
            List<Task> weeklyTasks = new List<Task>();

            for (var i = 0; i < allTasks.Count; i++)
            {
                if(DateInsideOneWeek(dateTime, allTasks[i].Date)){
                    weeklyTasks.Add(allTasks[i]);
                }
            }

            return Ok(weeklyTasks);
        }

        [HttpGet("monthly")]
        public ActionResult GetThisMonth()
        {

            List<Task> allTasks = tasksRepository.GetAll();
            List<Task> monthlyTasks = new List<Task>();

            for (var i = 0; i < allTasks.Count; i++)
            {
                if(DateInsideOneMonth(DateTime.Today, allTasks[i].Date)){
                    monthlyTasks.Add(allTasks[i]);
                }
            }

            return Ok(monthlyTasks);
        }

        [HttpGet("monthly/{dateTime}")]
        public ActionResult GetAMonth(DateTime dateTime)
        {

            List<Task> allTasks = tasksRepository.GetAll();
            List<Task> monthlyTasks = new List<Task>();

            for (var i = 0; i < allTasks.Count; i++)
            {
                if(DateInsideOneMonth(dateTime, allTasks[i].Date)){
                    monthlyTasks.Add(allTasks[i]);
                }
            }

            return Ok(monthlyTasks);
        }

        [HttpGet("daily")]
        public ActionResult GetToday()
        {

            List<Task> allTasks = tasksRepository.GetAll();
            List<Task> dailyTasks = new List<Task>();

            for (var i = 0; i < allTasks.Count; i++)
            {
                if(DateInsideOneDay(DateTime.Today, allTasks[i].Date)){
                    dailyTasks.Add(allTasks[i]);
                }
            }

            return Ok(dailyTasks);
        }

        [HttpGet("daily/{dateTime}")]
        public ActionResult GetADay(DateTime dateTime)
        {

            List<Task> allTasks = tasksRepository.GetAll();
            List<Task> dailyTasks = new List<Task>();

            for (var i = 0; i < allTasks.Count; i++)
            {
                if(DateInsideOneDay(dateTime, allTasks[i].Date)){
                    dailyTasks.Add(allTasks[i]);
                }
            }

            return Ok(dailyTasks);
        }


        public static bool DateInsideOneWeek(DateTime date1, DateTime date2)
        {
            DayOfWeek firstDayOfWeek = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            DateTime startDateOfWeek = date1.Date;
            while (startDateOfWeek.DayOfWeek != firstDayOfWeek)
            { startDateOfWeek = startDateOfWeek.AddDays(-1d); }
            DateTime endDateOfWeek = startDateOfWeek.AddDays(6d);
            return date2 >= startDateOfWeek && date2 <= endDateOfWeek;
        }

        public static bool DateInsideOneMonth(DateTime date1, DateTime date2){
            return(date1.Month == date2.Month);
        }

        public static bool DateInsideOneDay(DateTime date1, DateTime date2){
            return(date1.Day == date2.Day);
        }
    }
}
