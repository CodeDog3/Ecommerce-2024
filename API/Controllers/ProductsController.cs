using System;
using Azure.Core.Pipeline;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductsController (IProductRepository _repo): ControllerBase
{
 


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> getProducts(string? brand, string? type, string? sort){
        return Ok(await _repo.GetProductsAsync(brand, type,sort));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id){
        var product = await _repo.GetProductByIdAsync(id);

        if(product == null) return NotFound();

        return product;

    } 

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct (Product product){
        _repo.AddProduct(product);
        if(await _repo.SaveChangesAsync()){
            return CreatedAtAction("GetProduct", new {id = product.Id}, product);
        }
        return BadRequest("Issue Creating Product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct (int id, Product product){
        if(product.Id != id || ProductExists(id) == false) return BadRequest("cannot update this product");

        _repo.UpdateProduct(product);

        if(await _repo.SaveChangesAsync()){
            return NoContent();
        }
        return BadRequest("Cannot update Product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduce(int id){
        var product = await _repo.GetProductByIdAsync(id);

        if(product == null) return BadRequest();

        _repo.DeleteProduct(product);

       if(await _repo.SaveChangesAsync()){
            return NoContent();
       }

       return BadRequest();
    }

    [HttpGet("{brands}")]
    public async Task<ActionResult<IEnumerable<string>>> GetBrands(){
        return Ok(await _repo.GetBrandsAsync());
    }

    [HttpGet("type")]
    public async Task<ActionResult<IEnumerable<string>>> GetTypes(){
        return Ok(await _repo.GetTypesAsync());
    }


    private bool ProductExists (int id){
        return _repo.ProductExists(id);
    }



}
