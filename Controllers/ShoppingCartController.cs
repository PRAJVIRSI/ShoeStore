using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStore_Group9.Data;
using ShoeStore_Group9.Models;
using System.Linq;

public class ShoppingCartController : Controller
{
    private readonly ApplicationDbContext _context;

    public ShoppingCartController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Cart()
    {
        var cartItems = _context.ShoppingCarts
            .Where(c => c.UserID == 1)
            .Include(c => c.Product)
            .ToList();

        return View(cartItems);
    }


[HttpPost]
    public IActionResult AddToCart(int id, int quantity)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductID == id);
        if (product == null)
        {
            return NotFound();
        }

        var cartItem = _context.ShoppingCarts
            .FirstOrDefault(c => c.ProductID == id && c.UserID == 1); 

        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
        }
        else
        {
            cartItem = new ShoppingCart
            {
                ProductID = id,
                Quantity = quantity,
                UserID = 1 
            };
            _context.ShoppingCarts.Add(cartItem);
        }

        _context.SaveChanges();
        return RedirectToAction("Cart");
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int id)
    {
        var cartItem = _context.ShoppingCarts.FirstOrDefault(c => c.CartItemID == id);
        if (cartItem != null)
        {
            _context.ShoppingCarts.Remove(cartItem);
            _context.SaveChanges();
        }

        return RedirectToAction("Cart");
    }
}
