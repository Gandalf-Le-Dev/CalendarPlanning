﻿namespace CalendarPlanning.Shared.Exceptions.StoreExceptions
{
    public class StoreSaveUpdateException : Exception
    {
        public StoreSaveUpdateException(Guid id, string message) : base($"Unable to update the store with id {id} in the database. \nMessage: {message}")
        {
        }
    }

}
