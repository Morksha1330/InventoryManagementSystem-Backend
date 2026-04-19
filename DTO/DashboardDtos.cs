namespace InventoryMgtSystem.DTO;

public class DashboardDtos
{
     // Kpi Cards
    public class DashboardKpiDto
    {
        public int     TotalUsers      { get; set; }
        public int     TotalProducts   { get; set; }
        public int     TotalCategories { get; set; }
        public int     TotalCustomers  { get; set; }
        public int     TotalSuppliers  { get; set; }
        public decimal TotalExpense    { get; set; }
        public decimal TotalSales      { get; set; }
    }
 
    // Top Selling 
    public class TopSellingProductDto
    {
        public int     ProductId      { get; set; }
        public string  ProductName    { get; set; } = string.Empty;
        public string  CategoryName   { get; set; } = string.Empty;
        public int     TotalUnitsSold { get; set; }
        public decimal TotalRevenue   { get; set; }
    }
 
    // Sales Trend
    public class SalesTrendDto
    {
        public DateTime TrendDate    { get; set; }
        public string   DayName      { get; set; } = string.Empty;
        public decimal  DailySales   { get; set; }
        public decimal  DailyExpense { get; set; }
    }
 
    // Low Stock Alert
    public class LowStockItemDto
    {
        public int     ProductId       { get; set; }
        public string  ProductName     { get; set; } = string.Empty;
        public string  CategoryName    { get; set; } = string.Empty;
        public string  SKU             { get; set; } = string.Empty;
        public int     QuantityInStock { get; set; }
        public decimal UnitPrice       { get; set; }
    }
 
    // Recently Added
    public class RecentUserDto
    {
        public int      Id          { get; set; }
        public string   Name        { get; set; } = string.Empty;
        public string   Username    { get; set; } = string.Empty;
        public string   Email       { get; set; } = string.Empty;
        public string   EPF_No      { get; set; } = string.Empty;
        public string   RoleName    { get; set; } = string.Empty;
        public bool     Active      { get; set; }
        public DateTime CreatedDate { get; set; }
    }
 
    // UserSplit
    public class UserSplitDto
    {
        public int NewUsers        { get; set; }
        public int SubscribedUsers { get; set; }
        public int TotalUsers      { get; set; }
    }
 
    //FULL Summary
    public class DashboardSummaryDto
    {
        public DashboardKpiDto             Kpi              { get; set; } = new();
        public List<TopSellingProductDto>  TopProducts      { get; set; } = new();
        public List<SalesTrendDto>         SalesTrend       { get; set; } = new();
        public List<LowStockItemDto>       LowStockItems    { get; set; } = new();
        public List<RecentUserDto>         RecentUsers      { get; set; } = new();
        public UserSplitDto                UserSplit        { get; set; } = new();
    }
 
    //Query parameters 
    public class DashboardQueryParams
    {
        public int LowStockThreshold { get; set; } = 10;
        public int RecentUserCount   { get; set; } = 5;
        public int TopProductCount   { get; set; } = 5;
        public int TrendDays         { get; set; } = 7;
    }
}