using OrderServices.Models;

namespace OrderServices.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
    }
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        public ProductRepository(OrderDbContext context) : base(context)
        {
        }
    }
}
