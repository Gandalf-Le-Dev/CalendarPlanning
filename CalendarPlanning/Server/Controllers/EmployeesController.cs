﻿using CalendarPlanning.Server.Exceptions;
using CalendarPlanning.Server.Services.Interfaces;
using CalendarPlanning.Shared.Models;
using CalendarPlanning.Shared.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;

namespace CalendarPlanning.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        // GET: api/<EmployeesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _employeesService.GetEmployeesAsync());
        }

        // GET: api/<EmployeesController>/{id}
        [HttpGet("{id:guid}", Name = "GetEmployeeById")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var employee = await _employeesService.GetEmployeeByIdAsync(id);
                return Ok(employee);
            }
            catch (EmployeeNotFoundException)
            {
                return NotFound();
            }
        }

        // POST api/<EmployeesController>
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] AddEmployeeRequest addEmployeeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var employee = await _employeesService.CreateEmployeeAsync(addEmployeeRequest);
                return CreatedAtRoute("GetEmployeeById", new { id = employee.EmployeeId }, employee);
            }
            catch (InvalidEmployeeRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        // PUT api/<EmployeesController>/5
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, UpdateEmployeeRequest updateEmployeeRequest)
        {
            try
            {
                await _employeesService.UpdateEmployeeAsync(id, updateEmployeeRequest);
                return NoContent();
            }
            catch (EmployeeNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidEmployeeRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<EmployeesController>/5
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _employeesService.DeleteEmployeeAsync(id);
                return NoContent();
            }
            catch (EmployeeNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
