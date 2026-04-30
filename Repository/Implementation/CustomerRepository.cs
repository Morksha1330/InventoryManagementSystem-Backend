using InventoryMgtSystem.Data;
using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace InventoryMgtSystem.Repository.Implementation
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public void Delete(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public async Task<Customer> GetCustomer(int id)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
                throw new KeyNotFoundException("Customer Not Found");

            return customer;
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
        }

        public async Task<PagedResultDto<CustomerDto>> GetPagedCustomersAsync(RequestFilterDto filter)
        {
            var query = _context.Customers
                .AsNoTracking()
                .AsQueryable();

            // Search Filter
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(c =>
                    c.CustomerName.Contains(filter.SearchTerm) ||
                    c.CustomerEmail.Contains(filter.SearchTerm) ||
                    c.PhoneNumber.Contains(filter.SearchTerm));
            }

            // Active Filter
            if (filter.Active.HasValue)
            {
                query = query.Where(c => c.Active == filter.Active.Value);
            }

            var totalCount = await query.CountAsync();

            // Sorting
            query = ApplySorting(query, filter.SortBy, filter.SortOrder);

            // Pagination
            var customers = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    CustomerName = c.CustomerName,
                    CustomerEmail = c.CustomerEmail,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Active = c.Active,
                    CreatedUser =  c.CreatedUser,
                    CreatedDate = c.CreatedDate
                })
                .ToListAsync();

            return new PagedResultDto<CustomerDto>
            {
                Items = customers,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        private IQueryable<Customer> ApplySorting(IQueryable<Customer> query, string sortBy, string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                sortBy = "Id";

            var isDescending = sortOrder?.ToUpper() == "DESC";

            return sortBy.ToLower() switch
            {
                "customername" => isDescending
                    ? query.OrderByDescending(c => c.CustomerName)
                    : query.OrderBy(c => c.CustomerName),

                "customeremail" => isDescending
                    ? query.OrderByDescending(c => c.CustomerEmail)
                    : query.OrderBy(c => c.CustomerEmail),

                "phonenumber" => isDescending
                    ? query.OrderByDescending(c => c.PhoneNumber)
                    : query.OrderBy(c => c.PhoneNumber),

                "createddate" => isDescending
                    ? query.OrderByDescending(c => c.CreatedDate)
                    : query.OrderBy(c => c.CreatedDate),

                "active" => isDescending
                    ? query.OrderByDescending(c => c.Active)
                    : query.OrderBy(c => c.Active),

                _ => isDescending
                    ? query.OrderByDescending(c => c.Id)
                    : query.OrderBy(c => c.Id)
            };
        }

        public async Task<bool> CustomerExistsAsync(string email, string phoneNumber, int? excludeId = null)
        {
            var query = _context.Customers.AsQueryable();

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return await query.AnyAsync(c =>
                c.CustomerEmail == email ||
                c.PhoneNumber == phoneNumber);
        }
    }
}