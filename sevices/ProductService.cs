using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using MyApiProject.contracts;
using MyApiProject.Data;
using MyApiProject.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http.HttpResults;


namespace MyApiProject.Services;

public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Product
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _context.Product
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == id);

            // We return the raw entity, which can be null if not found
            return  product;
        }

        public async Task<Product> CreateAsync(ProductModifyDto dto)
        {
            var newProduct = new Product
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId
            };

            _context.Product.Add(newProduct);
            await _context.SaveChangesAsync();

            // Return the newly created entity
            return newProduct;
        }

        public async Task<bool> UpdateAsync(int id, ProductUpdateDto dto)
        {
            var product = await _context.Product
                .FirstOrDefaultAsync(p => p.ProductId == id);

          

            if (product == null)
            {
                return false;
            }

            // ONLY apply changes if the DTO provided a non-null value for the field.
            
            if (dto.ProductName != null)
            {
                product.ProductName = dto.ProductName;
            }

            if (dto.Description != null)
            {
                product.Description = dto.Description;
            }

            // For value types (decimal, int), we check if the nullable type has a value.
            if (dto.Price != null)
            {
                product.Price = dto.Price;
            }

            if (dto.CategoryId != null)
            {
                product.CategoryId = dto.CategoryId;
            }
            
            try
            {
                // Only saves changes if any were applied.
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Product
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return false;
            }

            _context.Product.Remove(product);
            
            await _context.SaveChangesAsync();
            
            return true;
        }
    }