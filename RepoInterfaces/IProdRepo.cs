using Store444.Models;

namespace Store444.RepoInterfaces;

public interface IProdRepo
{
    Task<List<Product>> GetProductsAsync();
    Task<Product> GetProductWithIdAsync(int id);
    Task<List<Product>> GetShipperProductsAsync(string id);
    Task<int> CreateProductAsync(Product prod); 
    Task<int> EditProductAsync(Product prod); 
    Task<int> DeleteProductAsync(Product prod);
    Task<int> SaveChangesAsync();
}
