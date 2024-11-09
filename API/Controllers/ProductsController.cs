using System;
using Azure.Core.Pipeline;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductsController (IGenericRepository<Product> _repo): ControllerBase
{
 


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> getProducts(string? brand, string? type, string? sort){

        var spec = new ProductSpecification(brand,type,sort);
        var products = await _repo.ListAsync(spec);

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id){
        var product = await _repo.GetByIdAsync(id);

        if(product == null) return NotFound();

        return product;

    } 

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct (Product product){
        _repo.Add(product);
        if(await _repo.SaveAllAsync()){
            return CreatedAtAction("GetProduct", new {id = product.Id}, product);
        }
        return BadRequest("Issue Creating Product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct (int id, Product product){
        if(product.Id != id || ProductExists(id) == false) return BadRequest("cannot update this product");

        _repo.Update(product);

        if(await _repo.SaveAllAsync()){
            return NoContent();
        }
        return BadRequest("Cannot update Product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduce(int id){
        var product = await _repo.GetByIdAsync(id);

        if(product == null) return BadRequest();

        _repo.Remove(product);

       if(await _repo.SaveAllAsync()){
            return NoContent();
       }

       return BadRequest();
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IEnumerable<string>>> GetBrands(){
        var spec = new BrandListSpecification();
        return Ok(await _repo.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<string>>> GetTypes(){
        var spec = new TypeListSpecification();
        return Ok(await _repo.ListAsync(spec));
    }


    private bool ProductExists (int id){
        return _repo.Exists(id);
    }



}
