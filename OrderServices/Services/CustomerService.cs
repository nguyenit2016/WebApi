using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using OrderServices.Helpers;
using OrderServices.Models;
using OrderServices.Repositories;
using OrderServices.UnitOfWork;
using OrderServices.ViewModels;

namespace OrderServices.Services
{
    public interface ICustomerService
    {
        ApiResponse<List<CustomerResponse>> GetAll();
        Task<ApiResponse<CustomerResponse>> AddAsync(CustomerRequest customer);
        Task<ApiResponse<CustomerResponse>> UpdateAsync(int customerId, CustomerRequest customer);
        Task<ApiResponse<CustomerResponse>> DeleteAsync(int customerId);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        protected readonly OrderDbContext _context;
        public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, OrderDbContext context)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public ApiResponse<List<CustomerResponse>> GetAll()
        {
            var res = _context.Set<Customer>()
                .Select(c => new CustomerResponse
                {
                    CustomerId = c.CustomerId,
                    FullName = c.FullName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                })
                .AsNoTracking()
                .ToList();

            return ApiResponse<List<CustomerResponse>>.SuccessResponse(res);
        }

        public async Task<ApiResponse<CustomerResponse>> AddAsync(CustomerRequest customerRequest)
        {
            var customer = new Customer
            {
                FullName = customerRequest.FullName,
                Email = customerRequest.Email,
                PhoneNumber = customerRequest.PhoneNumber
            };
            _customerRepository.Add(customer);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                return ApiResponse<CustomerResponse>.FailureResponse("Failed to create customer.");
            }
            var createdCustomer = new CustomerResponse
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            };
            return ApiResponse<CustomerResponse>.SuccessResponse(createdCustomer);
        }

        public async Task<ApiResponse<CustomerResponse>> UpdateAsync(int customerId, CustomerRequest customerRequest)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customerId);

            if (existingCustomer == null)
            {
                return ApiResponse<CustomerResponse>.FailureResponse($"Customer with ID {customerId} not found.");
            }

            existingCustomer.FullName = customerRequest.FullName;
            existingCustomer.Email = customerRequest.Email;
            existingCustomer.PhoneNumber = customerRequest.PhoneNumber;

            //_customerRepository.Update(existingCustomer);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                return ApiResponse<CustomerResponse>.FailureResponse("Failed to update customer.");
            }

            var customer =  new CustomerResponse
            {
                CustomerId = existingCustomer.CustomerId,
                FullName = existingCustomer.FullName,
                Email = existingCustomer.Email,
                PhoneNumber = existingCustomer.PhoneNumber
            };
            return ApiResponse<CustomerResponse>.SuccessResponse(customer);
        }

        public async Task<ApiResponse<CustomerResponse>> DeleteAsync(int customerId)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customerId);
            if (existingCustomer == null) return ApiResponse<CustomerResponse>.FailureResponse($"Customer with ID {customerId} not found.");

            _customerRepository.Delete(existingCustomer);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                return ApiResponse<CustomerResponse>.FailureResponse("Failed to update customer.");
            }
            var customer = new CustomerResponse
            {
                CustomerId = existingCustomer.CustomerId,
                FullName = existingCustomer.FullName,
                Email = existingCustomer.Email,
                PhoneNumber = existingCustomer.PhoneNumber
            };
            return ApiResponse<CustomerResponse>.SuccessResponse(customer);
        }
    }
}
