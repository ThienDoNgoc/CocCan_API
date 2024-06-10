using CocCanService.DTOs.Product;
using CocCanService.Services.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocCanService.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductDTO>>> GetAllProductsAsync(string filter, string range, string sort);
        Task<ServiceResponse<ProductDTO>> CreateProductAsync(CreateProductDTO createProductDTO);
        Task<ServiceResponse<ProductDTO>> UpdateProductAsync(ProductDTO productDTO);
        Task<ServiceResponse<string>> SoftDeleteProductAsync(Guid id);
        Task<ServiceResponse<bool>> HardDeleteProductAsync(ProductDTO productDTO);
        Task<ServiceResponse<ProductDTO>> GetProductByGUIDAsync(Guid id); 
    }
}
