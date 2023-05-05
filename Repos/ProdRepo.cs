using Microsoft.EntityFrameworkCore;
using Store444.Contexts;
using Store444.Models;
using Store444.RepoInterfaces;

namespace Store444.Repos;

public class ProdRepo : IProdRepo
{
    private readonly DrugShopContext _context;

    public ProdRepo(DrugShopContext context)
    {
        _context = context;
    }
    public async Task<int> CreateProductAsync(Product prod)
    {
        await _context.Products.AddAsync(prod);
        return await SaveChangesAsync();
    }

    public async Task<int> DeleteProductAsync(Product prod)
    {
        _context.Products.Remove(prod);
        return await SaveChangesAsync();
    }

    public async Task<int> EditProductAsync(Product prod)
    {
        _context.Products.Update(prod);
        return await SaveChangesAsync();
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetProductWithIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<List<Product>> GetShipperProductsAsync(string id)
    {
        return await _context.Products.Where(p=>p.UserId == id).ToListAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
