﻿using CalendarPlanning.Shared.Models.DTO;
using CalendarPlanning.Shared.Models.Requests.IncentiveRequests;

namespace CalendarPlanning.Server.Services.Interfaces
{
    public interface IIncentivesService
    {
        Task<IEnumerable<IncentiveDto>> GetIncentivesAsync();
        Task<IEnumerable<IncentiveDto>> GetIncentivesOfUserById(string userId);
        Task<IncentiveDto> GetIncentiveByIdAsync(Guid id);
        Task<IncentiveDto> CreateIncentiveAsync(CreateIncentiveRequest createIncentiveRequest);
        Task UpdateIncentiveAsync(Guid id, UpdateIncentiveRequest updateIncentiveRequest);
        Task DeleteIncentiveAsync(Guid id);
        Task DeleteIncentiveOfUserByIdAsync(string userId, Guid id);
    }
}
