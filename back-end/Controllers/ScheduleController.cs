using ClinicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ClinicAPI.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController : Controller
    {
        [Authorize(Roles = Role.Admin + "," + Role.Employee)]
        [HttpGet]
        public IActionResult GetDays()
        {
            List<Schedule> schedules = new List<Schedule>();
            for (int i = 0; i < 180; i++)
            {
                Schedule schedule = new Schedule();
                schedule.Id = i;
                schedule.Date = DateTime.Today.AddDays(i).ToString("yyyy-MM-dd");
                schedules.Add(schedule);
            }
            return Json(schedules);
        }
    }
}
