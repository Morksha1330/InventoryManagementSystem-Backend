using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;

namespace InventoryMgtSystem.Services.Interface;

public interface IDashboardService
{
    Task<HttpResponseData<DashboardDtos.DashboardSummaryDto>> GetDashboardSummaryAsync(
        DashboardDtos.DashboardQueryParams queryParams,
        CancellationToken    cancellationToken = default);
}