using System;
using System.Collections.Generic;

namespace Product.Domain
{    
    public interface IProductDao
    {     
        List<ProductSummary> GetProducts();

        Guid GetProductId(String name, decimal price);

        ProductSummary GetProduct(Guid id);

        Guid AddProduct(ProductSummary productSummary);

        void UpdateProduct(ProductSummary productSummary);

        void DeleteProduct(Guid id);
    }
}
