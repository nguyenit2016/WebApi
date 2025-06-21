using Castle.Core.Resource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderServices.Helpers;
using OrderServices.Models;
using OrderServices.Repositories;
using OrderServices.UnitOfWork;
using OrderServices.ViewModels;

namespace OrderServices.Services
{
    public interface IOrderService
    {
        Task<ApiResponse<List<OrderResponse>>> GetAll(OrderFilterRequest filter);
        Task<ApiResponse<OrderDetailResponse>> CreateAsync(CreateOrderRequest order);
        Task<ApiResponse<List<OrderDetailResponse>>> GetByIdAsync(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        protected readonly OrderDbContext _context;
        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork, OrderDbContext context)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<ApiResponse<List<OrderResponse>>> GetAll(OrderFilterRequest filter)
        {
            var query = _context.Set<Order>().Include(o => o.Customer).AsQueryable();

            if (filter.CustomerId.HasValue)
                query = query.Where(o => o.CustomerId == filter.CustomerId.Value);

            if (filter.FromDate.HasValue)
                query = query.Where(o => o.OrderDate.Date >= filter.FromDate.Value.Date);

            if (filter.ToDate.HasValue)
                query = query.Where(o => o.OrderDate.Date <= filter.ToDate.Value.Date);

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderResponse
                {
                    OrderId = o.OrderId,
                    CustomerName = o.Customer.FullName,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount
                })
                .ToListAsync();

            return ApiResponse<List<OrderResponse>>.SuccessResponse(orders);
        }

        public async Task<ApiResponse<OrderDetailResponse>> CreateAsync(CreateOrderRequest order)
        {
            bool requestValid = true;
            var errors = new Dictionary<string, List<string>>();
            // Kiểm tra khách hàng có tồn tại không
            var customer = await _context.Set<Customer>().FindAsync(order.CustomerId);
            if (customer == null)
            {
                requestValid = false;
                errors.Add("CustomerId", new List<string> { $"Customer with ID {order.CustomerId} not found"});
            }
            // Kiểm tra sản phẩm có tồn tại không
            var productIds = order.OrderItems.Select(i => i.ProductId).ToList();
            var products = await _context.Set<Product>()
                .Where(p => productIds.Contains(p.ProductId))
                .ToDictionaryAsync(p => p.ProductId);
            var missingProducts = productIds.Except(products.Keys).ToList();
            if (missingProducts.Any())
            {
                requestValid = false;
                errors.Add("ProductId", missingProducts.Select(id => $"Product with ID {id} not found").ToList());
            }
            if (!requestValid)
            {
                return ApiResponse<OrderDetailResponse>.FailureResponse("Some products or customers not found", errors);
            }

            var totalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
            var orderEntity = new Order
            {
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                TotalAmount = totalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItem
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
            _orderRepository.Add(orderEntity);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                return ApiResponse<OrderDetailResponse>.FailureResponse("Failed to create order.");
            }
            var response = new OrderDetailResponse
            {
                OrderId = orderEntity.OrderId,
                CustomerId = orderEntity.CustomerId,
                OrderDate = orderEntity.OrderDate,
                TotalAmount = orderEntity.TotalAmount,
                OrderItems = orderEntity.OrderItems.Select(oi => new OrderItemResponse
                {
                    OrderId = orderEntity.OrderId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
            return ApiResponse<OrderDetailResponse>.SuccessResponse(response);
        }

        public async Task<ApiResponse<List<OrderDetailResponse>>> GetByIdAsync(int id)
        {
            var res = await _context.Set<Order>()
                .Where(o => o.OrderId == id)
                .Select(o => new OrderDetailResponse
                {
                    OrderId = o.OrderId,
                    CustomerId = o.CustomerId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    CustomerName = o.Customer.FullName,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemResponse
                    {
                        OrderId = o.OrderId,
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                })
                .AsNoTracking()
                .ToListAsync();
            return ApiResponse<List<OrderDetailResponse>>.SuccessResponse(res);
        }
    }
}
