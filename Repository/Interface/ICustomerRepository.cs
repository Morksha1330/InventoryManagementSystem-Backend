using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        Task<Customer> GetCustomer(int id);
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int id);
        Task<bool> Save();
        Task<PagedResultDto<CustomerDto>> GetPagedCustomersAsync(RequestFilterDto filter);
        Task<bool> CustomerExistsAsync(string email, string phoneNumber, int? excludeId = null);
    }
}