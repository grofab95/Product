using System;

namespace Product.Domain
{
    public class ProductSummary 
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public decimal Price { get; set; }
    }
}
