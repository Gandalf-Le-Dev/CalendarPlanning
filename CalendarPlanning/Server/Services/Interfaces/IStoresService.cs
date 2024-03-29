﻿using CalendarPlanning.Shared.Models.DTO;
using CalendarPlanning.Shared.Models.Requests.StoreRequests;

namespace CalendarPlanning.Server.Services.Interfaces
{
    public interface IStoresService
    {
        Task<IEnumerable<StoreDto>> GetStoresAsync();
        Task<StoreDto> GetStoreByIdAsync(Guid id);
        Task<StoreDto> CreateStoreAsync(CreateStoreRequest addStoreRequest);
        Task UpdateStoreAsync(Guid id, UpdateStoreRequest updateStoreRequest);
        Task DeleteStoreAsync(Guid id);
    }
}
