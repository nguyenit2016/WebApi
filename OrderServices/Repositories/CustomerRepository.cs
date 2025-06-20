using OrderServices.Models;

namespace OrderServices.Repositories
{
    public interface ICustomerRepository: IRepository<Customer>
    {
    }

    public class CustomerRepository: Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(OrderDbContext context) : base(context)
        {
        }
    }
}
