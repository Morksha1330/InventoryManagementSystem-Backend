using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Repository.Interface;
using InventoryMgtSystem.Services.Interface;

namespace InventoryMgtSystem.Services.Implementation;

public class DashboardService : IDashboardService
{
    
    private readonly IDashboardRepository _repo;
    private readonly ILogger<DashboardService> _logger;
 
    public DashboardService(IDashboardRepository repo,ILogger<DashboardService> logger)
    {
        _repo   = repo;
        _logger = logger;
    }
    public async Task<HttpResponseData<DashboardDtos.DashboardSummaryDto>> GetDashboardSummaryAsync(DashboardDtos.DashboardQueryParams queryParams, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate / clamp params so callers cannot request absurd ranges
            queryParams.LowStockThreshold = Math.Clamp(queryParams.LowStockThreshold, 1, 100);
            queryParams.RecentUserCount   = Math.Clamp(queryParams.RecentUserCount,   1,  50);
            queryParams.TopProductCount   = Math.Clamp(queryParams.TopProductCount,   1,  20);
            queryParams.TrendDays         = Math.Clamp(queryParams.TrendDays,          1,  90);
 
            var data = await _repo.GetDashboardSummaryAsync(queryParams, cancellationToken);
 
            return new HttpResponseData<DashboardDtos.DashboardSummaryDto>
            {
                Result      = data,
                ResponsCode = 200,
                Success     = true,
                Message     = "Dashboard data loaded successfully."
            };
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Dashboard request was cancelled by the client.");
            return new HttpResponseData<DashboardDtos.DashboardSummaryDto>
            {
                ResponsCode = 499,
                Success     = false,
                Error       = "Request cancelled."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load dashboard summary data.");
            return new HttpResponseData<DashboardDtos.DashboardSummaryDto>
            {
                ResponsCode = 500,
                Success     = false,
                Error       = "An error occurred while loading dashboard data."
            };
        }
    }
}