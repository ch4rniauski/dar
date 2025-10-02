using laba2.DTO;
using laba2.Models;
using Microsoft.AspNetCore.Mvc;

namespace laba2.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductsController : ControllerBase
{
    private static readonly List<Product> _products = [];

    [HttpGet]
    public IActionResult GetAll()
        => Ok(_products);

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var p = _products.FirstOrDefault(p => p.Id == id);

        if (p is null)
        {
            return NotFound();
        }
        
        return Ok(p);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ProductToCreateDto request)
    {
        var id = _products.Count == 0
            ? 0
            : _products.Max(p => p.Id) + 1;

        var product = new Product(
            id,
            request.Name,
            request.Description,
            request.Price);
        
        _products.Add(product);
        
        return Ok(product);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] ProductToUpdateDto request)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        
        if (product is null)
        {
            return NotFound();
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        
        return Ok(product);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var idx = _products.FindIndex(p => p.Id == id);
        
        if (idx < 0)
        {
            return NotFound();
        }
        
        _products.RemoveAt(idx);
        
        return Ok();
    }
}
