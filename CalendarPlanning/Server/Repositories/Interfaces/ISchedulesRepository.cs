﻿using CalendarPlanning.Shared.Models;
using CalendarPlanning.Shared.Models.Requests.ScheduleRequests;

namespace CalendarPlanning.Server.Repositories.Interfaces
{
    public interface ISchedulesRepository
    {
        Task<IEnumerable<Schedule>> GetSchedulesAsync();
        Task<Schedule?> GetScheduleByIdAsync(Guid id);
        Task<bool> CreateScheduleAsync(Schedule schedule);
        Task<bool> UpdateScheduleAsync(Guid id, UpdateScheduleRequest updateScheduleRequest);
        Task<bool> DeleteScheduleAsync(Guid id);
    }
}
