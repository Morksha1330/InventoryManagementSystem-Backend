using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models.Entities;

namespace InventoryMgtSystem.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        Task<User> GetUser(int id);
        void Add(User user);
        void Update(User user);
        void Delete(int id);
        Task<bool> Save();
        Task<PagedResultDto<UserDto>> GetPagedUsersAsync(UserFilterDto filter);
        Task<bool> UserExistsAsync(string username, string email, int? excludeId = null);
    }
}
