using Dapper;
using System.Collections.Generic;
using Product.Domain;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Product.MsSqlPersistance
{
    public class ProductDao : IProductDao
    {
        private readonly IDbConnection _db;

        public ProductDao()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);            
        }

        public List<ProductSummary> GetProducts()
        {
            return this._db.Query<ProductSummary>
                ("SELECT [Id],[Name],[Price] " +
                "FROM [PRODUCTS]").ToList();
        }

        public Guid GetProductId(String name, decimal price)
        {
            return _db.Query<Guid>("SELECT[Id] FROM [PRODUCTS] WHERE Name =@Name AND Price = @Price",
                new { Name = name, Price = price }).SingleOrDefault();
        }

        public ProductSummary GetProduct(Guid id)
        {
            return _db.Query<ProductSummary>("SELECT[id],[Name],[Price] FROM [PRODUCTS] WHERE Id =@Id",
                new { Id = id }).SingleOrDefault();
        }

        public Guid AddProduct(ProductSummary productSummary)
        {
            this._db.Execute(
                @"INSERT Products([Name],[Price]) 
                    values (@name, @price)",
                new
                {
                    name = productSummary.Name, 
                    price = productSummary.Price
                });
            return GetProductId(productSummary.Name, productSummary.Price);
        }
        
        public void UpdateProduct(ProductSummary productSummary)
        {
            var query = "UPDATE PRODUCTS SET Price = @price, " +
                "Name = '" + productSummary.Name + "'" +
                "WHERE Id = '" + productSummary.Id + "' ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@price", dbType: DbType.Decimal,
                value: productSummary.Price, direction: ParameterDirection.Input, precision: 10, scale: 2);
            using (var connection = _db)
            {
                IEnumerable<dynamic> results = connection.Query(query, dynamicParameters);
            }
        }

        public void DeleteProduct(Guid id)
        {
            this._db.Execute(@"DELETE FROM [PRODUCTS] WHERE id = @id",
                new { Id = id });
        }
    }
}
