using System;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController (IGenericRepository<Product> _repo): BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> getProducts([FromQuery]ProductSpecParams specParams){

        var spec = new ProductSpecification(specParams);

        return await CreatePagedResults(_repo, spec, specParams.PageIndex, specParams.PageSize);
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
