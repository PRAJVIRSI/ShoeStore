using Microsoft.AspNetCore.Mvc;
using ShoeStore_Group9.Data;
using ShoeStore_Group9.Models;
using System.Linq;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products.ToList();
        return View(products);
    }

    public IActionResult Products()
    {
        var products = _context.Products.Take(10).ToList(); 
        return View(products);
    }

    public IActionResult ProductDetails(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductID == id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
}
