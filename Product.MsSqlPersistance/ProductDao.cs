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
        public List<ProductSummary> GetProducts()
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return db.Query<ProductSummary>("SELECT [Id],[Name],[Price] FROM [PRODUCTS]").ToList();
            }
        }

        public ProductSummary GetProduct(Guid id)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return db.QuerySingleOrDefault<ProductSummary>("SELECT[id],[Name],[Price] FROM [PRODUCTS] WHERE Id =@Id",
                new { Id = id });
            }
        }

        public Guid AddProduct(ProductSummary productSummary)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return db.Query<Guid>(
                @"DECLARE @InsertedRows AS TABLE (Id uniqueidentifier);
                    INSERT INTO Products ([Name],[Price]) OUTPUT Inserted.Id INTO @InsertedRows
                    VALUES (@name, @price);
                    SELECT Id FROM @InsertedRows",
                new
                {
                    name = productSummary.Name,
                    price = productSummary.Price
                }).Single();
            }
        }

        public void UpdateProduct(ProductSummary productSummary)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                db.Execute(@"UPDATE [dbo].[Products] SET Name = @name, Price = @price WHERE Id = @id", new
                {
                    id = productSummary.Id,
                    name = productSummary.Name,
                    price = productSummary.Price
                });
            }
        }

        public void DeleteProduct(Guid id)
        {
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                db.Execute(@"DELETE FROM [PRODUCTS] WHERE id = @id",
                new { Id = id });
            }
        }
    }
}
