﻿using CalendarPlanning.Server.Services.Interfaces;
using CalendarPlanning.Shared.Models;
using CalendarPlanning.Shared.Models.Requests;

namespace CalendarPlanning.Server.Services
{
    public class StoreService : IStoreService
    {
        public Task<bool> CreateStoreAsync(Store store)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteStoreAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Store?> GetStoreByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Store>> GetStoresAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStoreAsync(Guid id, UpdateStoreRequest updateStoreRequest)
        {
            throw new NotImplementedException();
        }
    }
}
