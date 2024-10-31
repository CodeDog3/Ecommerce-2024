using System;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly StoreContext _context;
    public ProductsController (StoreContext context){
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> getProducts(){
        return await _context.Products.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id){
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if(product == null) return NotFound();
        return product;

    } 

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct (Product product){
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct (int id, Product product){
        if(product.Id != id || await ProductExists(id) == false) return BadRequest("cannot update this product");

        _context.Entry(product).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduce(int id){
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if(product == null) return BadRequest();

        _context.Products.Remove(product);

       await _context.SaveChangesAsync();

       return NoContent();
    }

    private async Task<bool> ProductExists (int id){
        return await _context.Products.AnyAsync(x => x.Id == id);
    }



}
