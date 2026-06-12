using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface IAuditLogRepository
{
    Task<IEnumerable<AuditLog>> GetAllAsync();
}
