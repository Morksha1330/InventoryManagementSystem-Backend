using System.Data;
using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Repository.Interface;
using Microsoft.Data.SqlClient;

namespace InventoryMgtSystem.Repository.Implementation;

public class DashboardRepository : IDashboardRepository
{
    private readonly string  _connectionString;
        private readonly ILogger<DashboardRepository> _logger;
        private IDashboardRepository _dashboardRepositoryImplementation;

        public DashboardRepository(IConfiguration configuration,ILogger<DashboardRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection")
                                ?? throw new InvalidOperationException(
                                    "Connection string 'DefaultConnection' is not configured.");
            _logger = logger;
        }
 
        public async Task<DashboardDtos.DashboardSummaryDto> GetDashboardSummaryAsync(
            DashboardDtos.DashboardQueryParams queryParams,
            CancellationToken    cancellationToken = default)
        {
            var summary = new DashboardDtos.DashboardSummaryDto();
 
            await using var connection = new SqlConnection(_connectionString);
            await using var command    = new SqlCommand("usp_GetDashboardData", connection)
            {
                CommandType    = CommandType.StoredProcedure,
                CommandTimeout = 30
            };
 
            // Map query params → SP parameters
            command.Parameters.AddWithValue("@LowStockThreshold", queryParams.LowStockThreshold);
            command.Parameters.AddWithValue("@RecentUserCount",   queryParams.RecentUserCount);
            command.Parameters.AddWithValue("@TopProductCount",   queryParams.TopProductCount);
            command.Parameters.AddWithValue("@TrendDays",         queryParams.TrendDays);
 
            await connection.OpenAsync(cancellationToken);
 
            await using var reader = await command.ExecuteReaderAsync(
                CommandBehavior.SequentialAccess, cancellationToken);
 
            // ── Result Set #1 : KPI Counts ───────────────────
            if (await reader.ReadAsync(cancellationToken))
            {
                summary.Kpi = new DashboardDtos.DashboardKpiDto
                {
                    TotalUsers      = reader.GetInt32(reader.GetOrdinal("TotalUsers")),
                    TotalProducts   = reader.GetInt32(reader.GetOrdinal("TotalProducts")),
                    TotalCategories = reader.GetInt32(reader.GetOrdinal("TotalCategories")),
                    TotalCustomers  = reader.GetInt32(reader.GetOrdinal("TotalCustomers")),
                    TotalSuppliers  = reader.GetInt32(reader.GetOrdinal("TotalSuppliers")),
                    TotalExpense    = reader.GetDecimal(reader.GetOrdinal("TotalExpense")),
                    TotalSales      = reader.GetDecimal(reader.GetOrdinal("TotalSales")),
                };
            }
 
            // ── Result Set #2 : Top Selling Products ─────────
            await reader.NextResultAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                summary.TopProducts.Add(new DashboardDtos.TopSellingProductDto
                {
                    ProductId      = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    ProductName    = reader.GetString(reader.GetOrdinal("ProductName")),
                    CategoryName   = reader.GetString(reader.GetOrdinal("CategoryName")),
                    TotalUnitsSold = reader.GetInt32(reader.GetOrdinal("TotalUnitsSold")),
                    TotalRevenue   = reader.GetDecimal(reader.GetOrdinal("TotalRevenue")),
                });
            }
 
            // ── Result Set #3 : Sales Trend ──────────────────
            await reader.NextResultAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                summary.SalesTrend.Add(new DashboardDtos.SalesTrendDto
                {
                    TrendDate    = reader.GetDateTime(reader.GetOrdinal("TrendDate")),
                    DayName      = reader.GetString(reader.GetOrdinal("DayName")),
                    DailySales   = reader.GetDecimal(reader.GetOrdinal("DailySales")),
                    DailyExpense = reader.GetDecimal(reader.GetOrdinal("DailyExpense")),
                });
            }
 
            // ── Result Set #4 : Low Stock Items ──────────────
            await reader.NextResultAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                summary.LowStockItems.Add(new DashboardDtos.LowStockItemDto
                {
                    ProductId       = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    ProductName     = reader.GetString(reader.GetOrdinal("ProductName")),
                    CategoryName    = reader.GetString(reader.GetOrdinal("CategoryName")),
                    SKU             = reader.GetString(reader.GetOrdinal("SKU")),
                    QuantityInStock = reader.GetInt32(reader.GetOrdinal("QuantityInStock")),
                    UnitPrice       = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                });
            }
 
            // ── Result Set #5 : Recent Users ─────────────────
            await reader.NextResultAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                summary.RecentUsers.Add(new DashboardDtos.RecentUserDto
                {
                    Id          = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name        = reader.GetString(reader.GetOrdinal("Name")),
                    Username    = reader.GetString(reader.GetOrdinal("Username")),
                    Email       = reader.GetString(reader.GetOrdinal("Email")),
                    EPF_No      = reader.GetString(reader.GetOrdinal("EPF_No")),
                    RoleName    = reader.GetString(reader.GetOrdinal("RoleName")),
                    Active      = reader.GetBoolean(reader.GetOrdinal("Active")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                });
            }
 
            // ── Result Set #6 : User Split ───────────────────
            await reader.NextResultAsync(cancellationToken);
            if (await reader.ReadAsync(cancellationToken))
            {
                summary.UserSplit = new DashboardDtos.UserSplitDto
                {
                    NewUsers        = reader.GetInt32(reader.GetOrdinal("NewUsers")),
                    SubscribedUsers = reader.GetInt32(reader.GetOrdinal("SubscribedUsers")),
                    TotalUsers      = reader.GetInt32(reader.GetOrdinal("TotalUsers")),
                };
            }
 
            _logger.LogInformation(
                "Dashboard data loaded — Users:{U} Products:{P} LowStock:{LS}",
                summary.Kpi.TotalUsers, summary.Kpi.TotalProducts, summary.LowStockItems.Count);
 
            return summary;
        }
    }
