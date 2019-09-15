using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Product.Api.Models;
using Product.Domain;

namespace Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProductDao _productDao;

        public ProductController(IProductDao productDao)
        {
            _productDao = productDao;
        }

        [HttpGet]
        public IEnumerable<ProductSummary> Get()
        {
            return _productDao.GetProducts();
        }

        [HttpGet("{id}", Name = "Get")]
        public ProductSummary Get(Guid id)
        {
            return _productDao.GetProduct(id);
        }

        [HttpPost]
        public Guid Post(ProductCreateInputModel model)
        {
            var product = new ProductSummary
            {
                Name = model.Name,
                Price = model.Price
            };           
            return _productDao.AddProduct(product);
        }

        [HttpPut]
        public void Put(ProductUpdateInputModel model)
        {
            var product = new ProductSummary
            {
                Id = model.Id,
                Name = model.Name,
                Price = model.Price
            };
            _productDao.UpdateProduct(product);
        }
                
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            _productDao.DeleteProduct(id);
        }
    }
}
