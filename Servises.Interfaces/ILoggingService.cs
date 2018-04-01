using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Core;
using Domain.DataBaseModels.Logging;

namespace Servises.Interfaces
{
    /// <summary>
    /// Service for logging events.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Get list of events.
        /// </summary>
        /// <param name="count">Count of events to load.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the List ServerEvent of the operation.</returns>
        Task<List<ServerEvent>> GetListEventsAsync(int? count = null, int? offset = null, DateTime? fromDate = null, DateTime? toDate = null);

        /// <summary>
        /// Add server event to the backing store.
        /// </summary>
        /// <param name="serverEvent">The server event to add.</param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, 
        /// containing the ServiceResult of the operation.</returns>
        Task<ServiceResult> AddEventAsync(ServerEvent serverEvent);
    }
}