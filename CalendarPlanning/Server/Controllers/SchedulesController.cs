﻿using CalendarPlanning.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalendarPlanning.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly ISchedulesService _scheduleService;

        public SchedulesController(ISchedulesService scheduleService)
        {
            _scheduleService = scheduleService;
        }
    }
}
