using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Services.Interface
{
    public interface ICustomerService
    {
        Task<HttpResponseData<CustomerDto>> GetCustomerByIdAsync(int id);
        Task<HttpResponseData<PagedResultDto<CustomerDto>>> GetPagedCustomersAsync(RequestFilterDto filter);
        Task<HttpResponseData<Customer>> CreateCustomerAsync(Customer customer);
        Task<HttpResponseData<Customer>> UpdateCustomerAsync(Customer customer);
        Task<HttpResponseData<bool>> DeleteCustomerAsync(int id);
        Task<HttpResponseData<bool>> ToggleCustomerStatusAsync(int id);
    }
}