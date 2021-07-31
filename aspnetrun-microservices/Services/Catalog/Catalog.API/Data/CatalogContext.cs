using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatelogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            MongoClient client = new(
                configuration["DatabaseSettings:ConnectionString"]
            );

            var database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);

            Products = database.GetCollection<Product>(configuration["DatabaseSettings:CollectionName"]);

            CatalogConextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
