using InventoryMgtSystem.Data;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using InventoryMgtSystem.DTO;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private static readonly HashSet<string> _revokedTokens = new();


        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
                
            if (user == null)
                throw new KeyNotFoundException("User Not Found");
                   
            return user;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PagedResultDto<UserDto>> GetPagedUsersAsync(UserFilterDto filter)
        {
            var query = _context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(u => 
                    u.Name.Contains(filter.SearchTerm) ||
                    u.Username.Contains(filter.SearchTerm) ||
                    u.Email.Contains(filter.SearchTerm) ||
                    u.EPF_No.Contains(filter.SearchTerm));
            }

            if (filter.RoleId.HasValue && filter.RoleId.Value > 0)
            {
                query = query.Where(u => u.RoleId == filter.RoleId.Value);
            }

            if (filter.Active.HasValue)
            {
                query = query.Where(u => u.Active == filter.Active.Value);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = ApplySorting(query, filter.SortBy, filter.SortOrder);

            // Apply pagination
            var users = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Username = u.Username,
                    Email = u.Email,
                    EPF_No = u.EPF_No,
                    RoleId = u.RoleId,
                    RoleName = u.Role != null ? u.Role.RoleName : string.Empty,
                    Active = u.Active,
                    InitialAttempt = u.InitialAttempt,
                    CreatedUser = u.CreatedUser,
                    CreatedDate = u.CreatedDate,
                    Phone = u.PhoneNo
                    
                })
                .ToListAsync();

            return new PagedResultDto<UserDto>
            {
                Items = users,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        private IQueryable<User> ApplySorting(IQueryable<User> query, string sortBy, string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                sortBy = "Id";

            var isDescending = sortOrder?.ToUpper() == "DESC";

            return sortBy.ToLower() switch
            {
                "name" => isDescending ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
                "username" => isDescending ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username),
                "email" => isDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "epf_no" => isDescending ? query.OrderByDescending(u => u.EPF_No) : query.OrderBy(u => u.EPF_No),
                "rolename" => isDescending ? query.OrderByDescending(u => u.Role.RoleName) : query.OrderBy(u => u.Role.RoleName),
                "active" => isDescending ? query.OrderByDescending(u => u.Active) : query.OrderBy(u => u.Active),
                "createddate" => isDescending ? query.OrderByDescending(u => u.CreatedDate) : query.OrderBy(u => u.CreatedDate),
                _ => isDescending ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id)
            };
        }
        
        public async Task<bool> UserExistsAsync(string username, string email, int? excludeId = null)
        {
            var query = _context.Users.AsQueryable();

            if (excludeId.HasValue)
            {
                query = query.Where(u => u.Id != excludeId.Value);
            }

            return await query.AnyAsync(u => u.Username == username || u.Email == email);
        }

        public async Task<User> GetUserProfileAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new KeyNotFoundException("User Not Found");

            return user;
        }


        public Task<bool> RevokeTokenAsync(string token)
        {
            _revokedTokens.Add(token);
            return Task.FromResult(true);
        }

        public Task<bool> IsTokenRevokedAsync(string token)
        {
            return Task.FromResult(_revokedTokens.Contains(token));
        }
    }
}
