using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStore_Group9.Data;
using ShoeStore_Group9.Models;
using System.Linq;

public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrderController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Checkout()
    {
        return View();
    }

    [HttpPost]
    public IActionResult PlaceOrder(Order order)
    {
        var cartItems = _context.ShoppingCarts
            .Where(c => c.UserID == 1)
            .Include(c => c.Product)  
            .ToList();

        if (!cartItems.Any())
        {
            ModelState.AddModelError("", "Your cart is empty!");
            return View("Checkout", order);
        }

        order.UserID = 1; 
        order.OrderDate = DateTime.Now;
        order.TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity);

        _context.Orders.Add(order);
        _context.SaveChanges();

        foreach (var item in cartItems)
        {
            var orderItem = new OrderItem
            {
                OrderID = order.OrderID,
                ProductID = item.ProductID,
                Quantity = item.Quantity,
                Price = item.Product.Price
            };

            _context.OrderItems.Add(orderItem);
        }

        _context.ShoppingCarts.RemoveRange(cartItems);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }
}
