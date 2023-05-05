using Microsoft.EntityFrameworkCore;
using Store444.Contexts;
using Store444.Models;
using Store444.RepoInterfaces;

namespace Store444.Repos;

public class OrderRepo : IOrderRepo
{
    private readonly DrugShopContext _context;

    public OrderRepo(DrugShopContext context)
    {
        _context = context;
    }
    public async Task<int> DeleteAsync(Order order)
    {
        _context.Orders.Remove(order);
        return await SaveChangesAsync();
    }

    public async Task<int> EditAsync(Order order)
    {
        _context.Orders.Update(order);
        return await SaveChangesAsync();
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _context.Orders.Include(o => o.ShipTypeNavigation).Include(o => o.PaymentType).ToListAsync();
    }

    public async Task<Order> GetOrderWithIdAsync(int id)
    {
        return await _context.Orders.Include(o => o.ShipTypeNavigation).Include(o => o.PaymentType)
            .Where(o => o.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<PaymentType>> GetPaymentsAsync()
    {
        return await _context.PaymentTypes.ToListAsync();
    }

    public async Task<List<Product>> GetProductsWithIdAsync(int[] id)
    {
        var prods = new List<Product>();
        for (int i = 0; i < id.Length; i++)
        {
            prods.Add(await GetProductWithIdAsync(id[i]));
        }
        return prods;
    }

    public async Task<Product> GetProductWithIdAsync(int id)
    {
        return await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<ShipType>> GetShipTypesAsync()
    {
        return await _context.ShipTypes.ToListAsync();
    }

    public async Task<List<Order>> GetUserOrdersAsync(string id)
    {
        return await _context.Orders.Include(o => o.ShipTypeNavigation).Include(o => o.PaymentType)
            .Where(o => o.UserId == id).ToListAsync();
    }

    public async Task OrderCreateAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
