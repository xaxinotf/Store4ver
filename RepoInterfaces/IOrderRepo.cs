using Store444.Models;

namespace Store444.RepoInterfaces;

public interface IOrderRepo
{
    Task<List<Order>> GetOrdersAsync();
    Task<List<Order>> GetUserOrdersAsync(string id);
    Task<List<PaymentType>> GetPaymentsAsync();
    Task<List<ShipType>> GetShipTypesAsync();
    Task<Product> GetProductWithIdAsync(int id);
    Task<List<Product>> GetProductsWithIdAsync(int[] id);
    Task<Order> GetOrderWithIdAsync(int id);
    Task OrderCreateAsync(Order order);
    Task<int> SaveChangesAsync();
    Task<int> DeleteAsync(Order order);
    Task<int> EditAsync(Order order);
}
