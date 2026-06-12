using System.Collections.Generic;
using System.Threading.Tasks;
using WMS.Application.DTOs.AuditLog;

namespace WMS.Application.Interfaces;

public interface IAuditLogService
{
    Task<IEnumerable<AuditLogDto>> GetAllAsync();
}
