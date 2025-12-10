using Microsoft.AspNetCore.Mvc;
using MyApiProject.contracts;
namespace MyApiProject.Controllers;




    [ApiController]
    [Route("api/[controller]")] // Base route: /api/products
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        // Dependency Injection of the Product Service
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Retrieves a list of all products. (GET /api/products)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products); // 200 OK
        }

        /// <summary>
        /// Retrieves a specific product by ID. (GET /api/products/5)
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(); // 404 Not Found
            }

            return Ok(product); // 200 OK
        }

        /// <summary>
        /// Creates a new product. (POST /api/products)
        /// </summary>
        /// <param name="dto">The data required for product creation.</param>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModifyDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad Request if DTO validation fails
            }

            var createdProduct = await _productService.CreateAsync(dto);

            // Returns 201 Created with a Location header pointing to the new resource
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.ProductId }, createdProduct);
        }

        /// <summary>
        /// Updates an existing product. (PUT /api/products/5)
        /// </summary>
        /// <param name="id">The ID of the product to update (from the route).</param>
        /// <param name="dto">The updated product data (from the body).</param>
        [HttpPut("{id}")] 
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto dto)
        {
            // Ensures the body data passes validation rules
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 Bad Request
            }

            // The service receives the ID from the route and the data from the body.
            // This logic is based on your previous successful implementation.
            var success = await _productService.UpdateAsync(id, dto); 
            
            if (!success)
            {
                return NotFound($"Product with ID {id} not found."); // 404 Not Found
            }

            // 204 No Content is the standard response for successful updates that don't return data
            return NoContent(); 
        }
        
        /// <summary>
        /// Deletes a specific product by ID. (DELETE /api/products/5)
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteAsync(id);

            if (!success)
            {
                return NotFound($"Product with ID {id} not found."); // 404 Not Found
            }

            // 204 No Content is the standard response for successful deletes
            return NoContent(); 
        }
    }