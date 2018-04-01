using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.EntityFrameworkCore;
using Domain.DataBaseModels.Logging;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces;
using Domain.Core;
using Utils;

namespace EntityFrameworkLoggingService
{
    public class LoggingService : ILoggingService
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly DbSet<ServerEvent> _events;

        public LoggingService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _events = _dbContext.ServerLog;
        }

        public Task<List<ServerEvent>> GetListEventsAsync(int? count = null, int? offset = null, DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var query = _events.AsQueryable();

            if (fromDate != null)
            {
                query = query.Where(e => e.Time >= fromDate);
            }

            if (toDate != null)
            {
                query = query.Where(e => e.Time <= toDate);
            }

            query = query.Skip(offset.GetValueOrDefault());

            if (count != null)
            {
                query = query.Take(count.Value);
            }

            return query.ToListAsync();
        }

        public async Task<ServiceResult> AddEventAsync(ServerEvent serverEvent)
        {
            var errors = AttributeValidator.Validation(serverEvent);
            if (errors.Count > 0)
            {
                return new ServiceResult(false, errors);
            }

            await _events.AddAsync(serverEvent);
            await _dbContext.SaveChangesAsync();

            return new ServiceResult(true);
        }
    }
}
