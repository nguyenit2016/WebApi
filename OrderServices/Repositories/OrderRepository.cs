using OrderServices.Models;

namespace OrderServices.Repositories
{
    public interface IOrderRepository: IRepository<Order>
    {
    }
    public class OrderRepository: Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext context) : base(context)
        {
        }
    }
}
