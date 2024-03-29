﻿using CalendarPlanning.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalendarPlanning.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidaysService _holidayService;

        public HolidaysController(IHolidaysService holidayService)
        {
            _holidayService = holidayService;
        }
    }
}
