using Microsoft.EntityFrameworkCore;
using OrderServices.Models;
using OrderServices.Repositories;
using OrderServices.UnitOfWork;

namespace OrderServices.Services
{
    public interface ICustomerService
    {
        List<Customer> GetAll();
        Customer GetById(int id);
        Task<Customer> Add(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
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

        public List<Customer> GetAll()
        {
            var res = _customerRepository.GetAll().Result.ToList();
            return res;
        }

        public Customer GetById(int id)
        {
            var res = _customerRepository.GetById(id);
            return res;
        }

        public async Task<Customer> Add(Customer customer)
        {
            _customerRepository.Add(customer);
            await _unitOfWork.SaveChangesAsync();
            return customer;
        }

        public async void Update(Customer customer)
        {
            _customerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async void Delete(Customer customer)
        {
            _customerRepository.Delete(customer);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
