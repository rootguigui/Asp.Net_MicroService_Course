using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateProduct(Product product) => await _context.Products.InsertOneAsync(product);

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName) => await _context.Products.Find(Builders<Product>.Filter.Eq(p => p.Category, categoryName)).ToListAsync();

        public async Task<IEnumerable<Product>> GetProductByName(string name) => await _context.Products.Find(Builders<Product>.Filter.ElemMatch(p => p.Name, name)).ToListAsync();

        public async Task<Product> GetProductById(string id) => await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetProducts() => await _context.Products.Find(props => true).ToListAsync();

        public async Task<bool> UpdateProduct(Product product)
        {
            var result = await _context.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            DeleteResult result = await _context.Products.DeleteOneAsync(Builders<Product>.Filter.Eq(p => p.Id, id));

            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
