using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Repository.Interface;
using InventoryMgtSystem.Services.Interface;

namespace InventoryMgtSystem.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<HttpResponseData<CustomerDto>> GetCustomerByIdAsync(int id)
        {
            var response = new HttpResponseData<CustomerDto>();

            try
            {
                var customer = await _customerRepository.GetCustomer(id);

                response.Result = new CustomerDto
                {
                    Id = customer.Id,
                    CustomerName = customer.CustomerName,
                    CustomerEmail = customer.CustomerEmail,
                    Address = customer.Address,
                    PhoneNumber = customer.PhoneNumber,
                    Active = customer.Active,
                    CreatedUser = customer.CreatedUser,
                    CreatedDate = customer.CreatedDate
                };

                response.Success = true;
                response.ResponsCode = 200;
                response.Message = "Customer retrieved successfully";
            }
            catch (KeyNotFoundException ex)
            {
                response.Success = false;
                response.ResponsCode = 404;
                response.Error = ex.Message;
                response.Message = "Customer not found";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ResponsCode = 500;
                response.Error = ex.Message;
                response.Message = "An error occurred while retrieving customer";
            }

            return response;
        }

        public async Task<HttpResponseData<PagedResultDto<CustomerDto>>> GetPagedCustomersAsync(RequestFilterDto filter)
        {
            var response = new HttpResponseData<PagedResultDto<CustomerDto>>();

            try
            {
                filter.PageNumber = Math.Max(1, filter.PageNumber);
                filter.PageSize = Math.Clamp(filter.PageSize, 1, 100);
                filter.SortBy = string.IsNullOrWhiteSpace(filter.SortBy) ? "Id" : filter.SortBy;
                filter.SortOrder = filter.SortOrder?.ToUpper() == "DESC" ? "DESC" : "ASC";

                var result = await _customerRepository.GetPagedCustomersAsync(filter);

                response.Result = result;
                response.Success = true;
                response.ResponsCode = 200;
                response.Message = "Customers retrieved successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ResponsCode = 500;
                response.Error = ex.Message;
                response.Message = "An error occurred while retrieving customers";
            }

            return response;
        }

        public async Task<HttpResponseData<Customer>> CreateCustomerAsync(Customer customer)
        {
            var response = new HttpResponseData<Customer>();

            try
            {
                var exists = await _customerRepository.CustomerExistsAsync(
                    customer.CustomerEmail,
                    customer.PhoneNumber);

                if (exists)
                {
                    response.Success = false;
                    response.ResponsCode = 409;
                    response.Error = "Customer already exists";
                    response.Message = "Customer email or phone number already exists";
                    return response;
                }

                customer.CreatedDate = DateTime.UtcNow;
                customer.Active = true;

                _customerRepository.Add(customer);

                await _customerRepository.Save();

                response.Result = customer;
                response.Success = true;
                response.ResponsCode = 201;
                response.Message = "Customer created successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ResponsCode = 500;
                response.Error = ex.Message;
                response.Message = "An error occurred while creating customer";
            }

            return response;
        }

        public async Task<HttpResponseData<Customer>> UpdateCustomerAsync(Customer customer)
        {
            var response = new HttpResponseData<Customer>();

            try
            {
                var existingCustomer = await _customerRepository.GetCustomer(customer.Id);

                if (existingCustomer == null)
                {
                    response.Success = false;
                    response.ResponsCode = 404;
                    response.Error = "Customer not found";
                    response.Message = "The specified customer does not exist";
                    return response;
                }

                var exists = await _customerRepository.CustomerExistsAsync(
                    customer.CustomerEmail,
                    customer.PhoneNumber,
                    customer.Id);

                if (exists)
                {
                    response.Success = false;
                    response.ResponsCode = 409;
                    response.Error = "Duplicate customer";
                    response.Message = "Email or phone number already taken";
                    return response;
                }

                existingCustomer.CustomerName = customer.CustomerName;
                existingCustomer.CustomerEmail = customer.CustomerEmail;
                existingCustomer.Address = customer.Address;
                existingCustomer.PhoneNumber = customer.PhoneNumber;

                if (customer.Active != existingCustomer.Active)
                {
                    existingCustomer.Active = customer.Active;
                }

                _customerRepository.Update(existingCustomer);

                await _customerRepository.Save();

                response.Result = existingCustomer;
                response.Success = true;
                response.ResponsCode = 200;
                response.Message = "Customer updated successfully";
            }
            catch (KeyNotFoundException)
            {
                response.Success = false;
                response.ResponsCode = 404;
                response.Error = "Customer not found";
                response.Message = "The specified customer does not exist";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ResponsCode = 500;
                response.Error = ex.Message;
                response.Message = "An error occurred while updating customer";
            }

            return response;
        }

        public async Task<HttpResponseData<bool>> DeleteCustomerAsync(int id)
        {
            var response = new HttpResponseData<bool>();

            try
            {
                var customer = await _customerRepository.GetCustomer(id);

                if (customer == null)
                {
                    response.Success = false;
                    response.ResponsCode = 404;
                    response.Error = "Customer not found";
                    response.Message = "The specified customer does not exist";
                    return response;
                }

                _customerRepository.Delete(id);

                var result = await _customerRepository.Save();

                response.Result = result;
                response.Success = result;
                response.ResponsCode = result ? 200 : 400;
                response.Message = result
                    ? "Customer deleted successfully"
                    : "Failed to delete customer";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ResponsCode = 500;
                response.Error = ex.Message;
                response.Message = "An error occurred while deleting customer";
            }

            return response;
        }

        public async Task<HttpResponseData<bool>> ToggleCustomerStatusAsync(int id)
        {
            var response = new HttpResponseData<bool>();

            try
            {
                var customer = await _customerRepository.GetCustomer(id);

                if (customer == null)
                {
                    response.Success = false;
                    response.ResponsCode = 404;
                    response.Error = "Customer not found";
                    response.Message = "The specified customer does not exist";
                    return response;
                }

                customer.Active = !customer.Active;

                _customerRepository.Update(customer);

                var result = await _customerRepository.Save();

                response.Result = result;
                response.Success = result;
                response.ResponsCode = result ? 200 : 400;
                response.Message = result
                    ? $"Customer status toggled to {(customer.Active ? "Active" : "Inactive")} successfully"
                    : "Failed to toggle customer status";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ResponsCode = 500;
                response.Error = ex.Message;
                response.Message = "An error occurred while toggling customer status";
            }

            return response;
        }
    }
}