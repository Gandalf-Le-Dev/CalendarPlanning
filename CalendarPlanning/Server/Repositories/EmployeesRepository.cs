﻿using CalendarPlanning.Server.Data;
using CalendarPlanning.Server.Repositories.Interfaces;
using CalendarPlanning.Shared.Exceptions.EmployeeExceptions;
using CalendarPlanning.Shared.ModelExtensions;
using CalendarPlanning.Shared.Models;
using CalendarPlanning.Shared.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace CalendarPlanning.Server.Repositories
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly APIDbContext _dbContext;
        private readonly ILogger<EmployeesRepository> _logger;

        public EmployeesRepository(APIDbContext dbContext, ILogger<EmployeesRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            var employeeDto = employee.ToDto();

            _logger.LogInformation("Employee created: {Employee}", employeeDto);

            return employeeDto;
        }

        public async Task DeleteEmployeeAsync(string employeeId)
        {
            var result = await _dbContext.Employees.Where(e => e.EmployeeId == employeeId)
                .ExecuteDeleteAsync();

            if (result == 0) throw new EmployeeNotFoundException(employeeId);

            _logger.LogInformation("Employee deleted: {EmployeeId}", employeeId);
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(string employeeId)
        {
            var employee = await _dbContext.Employees
                .Include(e => e.Store)
                //.Include(e => e.Shifts)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId)
                ?? throw new EmployeeNotFoundException(employeeId);

            return employee.ToDto();
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsNoTrackingAsync(string employeeId)
        {
            var employee = await _dbContext.Employees
                .AsNoTracking()
                .Include(e => e.Store)
                //.Include(e => e.Shifts)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId)
                ?? throw new EmployeeNotFoundException(employeeId);

            return employee.ToDto();
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync()
        {
            var employees = await _dbContext.Employees
                .Include(e => e.Store)
                //.Include(e => e.Shifts)
                .ToListAsync();

            return employees.Select(e => e.ToDto());
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsNoTrackingAsync()
        {
            var employees = await _dbContext.Employees
                .AsNoTracking()
                .Include(e => e.Store)
                //.Include(e => e.Shifts)
                .ToListAsync();

            return employees.Select(e => e.ToDto());
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _dbContext.Employees.Update(employee);

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Employee updated: {Employee}", employee);
            }
            catch (DbUpdateException ex)
            {
                throw new EmployeeSaveUpdateException(employee.EmployeeId, ex.Message);
            }
        }

        public Task<int> GetEmployeesCountAsync()
        {
            return _dbContext.Employees.CountAsync();
        }
    }
}