using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public interface ICatelogContext
    {
        IMongoCollection<Product> Products { get; }
    }
}
