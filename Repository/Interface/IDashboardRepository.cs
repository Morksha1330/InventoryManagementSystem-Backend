using InventoryMgtSystem.DTO;

namespace InventoryMgtSystem.Repository.Interface;

public interface IDashboardRepository
{
    Task<DashboardDtos.DashboardSummaryDto> GetDashboardSummaryAsync(DashboardDtos.DashboardQueryParams queryParams,CancellationToken cancellationToken = default);
}