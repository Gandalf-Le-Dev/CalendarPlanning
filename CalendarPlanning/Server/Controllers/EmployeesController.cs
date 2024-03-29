﻿using CalendarPlanning.Server.Authorization;
using CalendarPlanning.Server.Exceptions;
using CalendarPlanning.Server.Services.Interfaces;
using CalendarPlanning.Shared.Exceptions.EmployeeExceptions;
using CalendarPlanning.Shared.Models.Requests.EmployeeRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CalendarPlanning.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeesService employeesService, ILogger<EmployeesController> logger)
        {
            _employeesService = employeesService;
            _logger = logger;
        }

        // GET: api/<EmployeesController>
        [Authorize(Policy = Policies.ReadAccess)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _employeesService.GetEmployeesAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting employees on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.TraceId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorCode = StatusCodes.Status500InternalServerError,
                    error = "Error while getting employees",
                    message = ex.Message,
                    traceId = Activity.Current?.TraceId.ToString()
                });
            }
        }

        [Authorize(Policy = Policies.ReadAccess)]
        [HttpGet("count")]
        public async Task<IActionResult> GetEmployeesCount()
        {
            try
            {
                return Ok(await _employeesService.GetEmployeesCountAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting employees count on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.TraceId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorCode = StatusCodes.Status500InternalServerError,
                    error = "Error while getting employees count",
                    message = ex.Message,
                    traceId = Activity.Current?.TraceId.ToString()
                });
            }
        }

        // GET: api/<EmployeesController>/{id}
        [Authorize(Policy =Policies.ConcernedUser)]
        [HttpGet("{userId}", Name = "GetEmployeeById")]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                return Ok(await _employeesService.GetEmployeeByIdAsync(userId));
            }
            catch (EmployeeNotFoundException ex)
            {
                _logger.LogWarning("Employee with id {Id} was not found on machine {Machine}. TraceId: {TraceId}", userId, Environment.MachineName, Activity.Current?.TraceId);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting employee with id {Id} on machine {Machine}. TraceId: {TraceId}", userId, Environment.MachineName, Activity.Current?.TraceId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorCode = StatusCodes.Status500InternalServerError,
                    error = "Error while getting employee",
                    message = ex.Message,
                    traceId = Activity.Current?.TraceId.ToString()
                });
            }
        }

        // POST api/<EmployeesController>
        [Authorize(Policy =Policies.WriteAccess)]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest createEmployeeRequest)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state {ModelState} on machine {Machine}. TraceId: {TraceId}", ModelState, Environment.MachineName, Activity.Current?.TraceId);
                return BadRequest(ModelState);
            }

            try
            {
                var employee = await _employeesService.CreateEmployeeAsync(createEmployeeRequest);
                return CreatedAtRoute("GetEmployeeById", new { id = employee.EmployeeId }, employee);
            }
            catch (InvalidRequestException ex)
            {
                _logger.LogWarning("Invalid request {Request} on machine {Machine}. TraceId: {TraceId}", createEmployeeRequest, Environment.MachineName, Activity.Current?.TraceId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating employee on machine {Machine}. TraceId: {TraceId}", Environment.MachineName, Activity.Current?.TraceId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorCode = StatusCodes.Status500InternalServerError,
                    error = "Error while creating employee",
                    message = ex.Message,
                    traceId = Activity.Current?.TraceId.ToString()
                });
            }

        }

        // PUT api/<EmployeesController>/5
        [Authorize(Policy = Policies.WriteAccess)]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateEmployee(string userId, UpdateEmployeeRequest updateEmployeeRequest)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state {ModelState} on machine {Machine}. TraceId: {TraceId}", ModelState, Environment.MachineName, Activity.Current?.TraceId);
                return BadRequest(ModelState);
            }

            try
            {
                await _employeesService.UpdateEmployeeAsync(userId, updateEmployeeRequest);
                return NoContent();
            }
            catch (EmployeeNotFoundException)
            {
                _logger.LogWarning("Employee with id {Id} was not found on machine {Machine}. TraceId: {TraceId}", userId, Environment.MachineName, Activity.Current?.TraceId);
                return NotFound();
            }
            catch (EmployeeSaveUpdateException ex)
            {
                _logger.LogError(ex, "Error while updating employee with id {Id} on machine {Machine}. TraceId: {TraceId}", userId, Environment.MachineName, Activity.Current?.TraceId);
                return BadRequest(ex.Message);
            }
            catch (InvalidRequestException ex)
            {
                _logger.LogWarning("Invalid request {Request} on machine {Machine}. TraceId: {TraceId}", updateEmployeeRequest, Environment.MachineName, Activity.Current?.TraceId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating employee with id {Id} on machine {Machine}. TraceId: {TraceId}", userId, Environment.MachineName, Activity.Current?.TraceId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorCode = StatusCodes.Status500InternalServerError,
                    error = "Error while updating employee",
                    message = ex.Message,
                    traceId = Activity.Current?.TraceId.ToString()
                });
            }
        }

        // DELETE api/<EmployeesController>/5
        [Authorize(Policy = Policies.WriteAccess)]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                await _employeesService.DeleteEmployeeAsync(userId);
                return NoContent();
            }
            catch (EmployeeNotFoundException)
            {
                _logger.LogWarning("Employee with id {Id} was not found on machine {Machine}. TraceId: {TraceId}", userId, Environment.MachineName, Activity.Current?.TraceId);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting employee with id {Id} on machine {Machine}. TraceId: {TraceId}", userId, Environment.MachineName, Activity.Current?.TraceId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorCode = StatusCodes.Status500InternalServerError,
                    error = "Error while deleting employee",
                    message = ex.Message,
                    traceId = Activity.Current?.TraceId.ToString()
                });
            }
        }

        [Authorize(Policy = Policies.WriteAccess)]
        [HttpPut("{userId}/AddShiftToEmployee")]
        public async Task<IActionResult> AddShiftToEmployee(string userId, AddShiftToEmployeeRequest addShiftToEmployeeRequest)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state {ModelState} on machine {Machine}. TraceId: {TraceId}", ModelState, Environment.MachineName, Activity.Current?.TraceId);
                return BadRequest(ModelState);
            }

            try
            {
                await _employeesService.AddShiftToEmployeeAsync(userId, addShiftToEmployeeRequest);
                return NoContent();
            }
            catch (EmployeeNotFoundException)
            {
                _logger.LogWarning("Employee with id {Id} was not found on machine {Machine}. TraceId: {TraceId}", userId, Environment.MachineName, Activity.Current?.TraceId);
                return NotFound();
            }
            catch (EmployeeSaveUpdateException ex)
            {
                _logger.LogError(ex, "Error while adding shift to employee with id {Id} on machine {Machine}. TraceId: {TraceId}", userId, Environment.MachineName, Activity.Current?.TraceId);
                return BadRequest(ex.Message);
            }
            catch (InvalidRequestException ex)
            {
                _logger.LogWarning("Invalid request {Request} on machine {Machine}. TraceId: {TraceId}", addShiftToEmployeeRequest, Environment.MachineName, Activity.Current?.TraceId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding shift to employee with id {Id} on machine {Machine}. TraceId: {TraceId}", userId, Environment.MachineName, Activity.Current?.TraceId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    errorCode = StatusCodes.Status500InternalServerError,
                    error = "Error while adding shift to employee",
                    message = ex.Message,
                    traceId = Activity.Current?.TraceId.ToString()
                });
            }
        }
    }
}