using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoeStore_Group9.Data;
using ShoeStore_Group9.Models;
using ShoeStore_Group9.ViewModels;
using System;
using System.Linq;

namespace ShoeStore_Group9.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/AdminLogin
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        // POST: /Admin/AdminLogin
        [HttpPost]
        public IActionResult AdminLogin(AdminLoginViewModel model)
        {
            const string adminUsername = "admin";
            const string adminPassword = "admin123";

            if (model.Username == adminUsername && model.Password == adminPassword)
            {
                return RedirectToAction("ManageProducts");
            }

            ViewBag.ErrorMessage = "Invalid username or password";
            return View();
        }

        // GET: /Admin/ManageProducts
        public IActionResult ManageProducts()
        {
            try
            {
                var products = _context.Products.Include(p => p.Category).ToList();
                var categories = _context.Categories.ToList();
                var viewModel = new Tuple<IEnumerable<Product>, IEnumerable<Category>>(products, categories);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products and categories: {ex.Message}");
                return View(new Tuple<IEnumerable<Product>, IEnumerable<Category>>(Enumerable.Empty<Product>(), Enumerable.Empty<Category>()));
            }
        }

        // GET: /Admin/CreateProduct
        public IActionResult CreateProduct()
        {
            try
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Title = "Create Product";
                ViewBag.Action = "CreateProduct";
                ViewBag.ButtonText = "Create";
                return View(new Product());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CreateProduct view: {ex.Message}");
                return RedirectToAction("ManageProducts");
            }
        }

        // POST: /Admin/CreateProduct
        [HttpPost]
        public IActionResult CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    return RedirectToAction("ManageProducts");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating product: {ex.Message}");
                }
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // GET: /Admin/EditProduct/5
        public IActionResult EditProduct(int id)
        {
            try
            {
                var product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductID == id);
                if (product == null)
                {
                    return NotFound();
                }

                ViewBag.Categories = _context.Categories.ToList();
                return View(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading EditProduct view: {ex.Message}");
                return RedirectToAction("ManageProducts");
            }
        }

        // POST: /Admin/EditProduct/5
        [HttpPost]
        public IActionResult EditProduct(int id, Product product)
        {
            // Remove the Category validation if necessary
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                try
                {
                    // Find the existing product in the database
                    var existingProduct = _context.Products.Find(id);
                    if (existingProduct == null)
                    {
                        return NotFound(); // If the product doesn't exist, return a 404
                    }

                    // Update the existing product's properties
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.StockQuantity = product.StockQuantity;
                    existingProduct.CategoryID = product.CategoryID;  // Ensure CategoryID is updated
                    existingProduct.ImageURL = product.ImageURL;

                    // Save the changes to the database
                    _context.SaveChanges();

                    return RedirectToAction("ManageProducts");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating product: {ex.Message}");
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            else
            {
                Console.WriteLine("Model state is invalid.");
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                    }
                }
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product); // Return the view with the product model if there's an error
        }



        // POST: /Admin/DeleteProduct/5
        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var product = _context.Products.Find(id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    _context.SaveChanges();
                }

                return RedirectToAction("ManageProducts");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return RedirectToAction("ManageProducts");
            }
        }

        // GET: /Admin/CreateCategory
        public IActionResult CreateCategory()
        {
            try
            {
                ViewBag.Title = "Create Category";
                ViewBag.Action = "CreateCategory";
                ViewBag.ButtonText = "Create";
                return View(new Category());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CreateCategory view: {ex.Message}");
                return RedirectToAction("ManageProducts");
            }
        }

        // POST: /Admin/CreateCategory
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    return RedirectToAction("ManageProducts");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating category: {ex.Message}");
                }
            }

            return View(category);
        }

        // GET: /Admin/EditCategory/5
        public IActionResult EditCategory(int id)
        {
            try
            {
                var category = _context.Categories.Find(id);
                if (category == null)
                {
                    return NotFound();
                }

                return View(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading EditCategory view: {ex.Message}");
                return RedirectToAction("ManageProducts");
            }
        }

        // POST: /Admin/EditCategory/5
        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingCategory = _context.Categories.Find(category.CategoryID);
                    if (existingCategory != null)
                    {
                        existingCategory.CategoryName = category.CategoryName;
                        existingCategory.Description = category.Description;

                        _context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine($"Category with ID {category.CategoryID} not found.");
                    }
                    return RedirectToAction("ManageProducts");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating category: {ex.Message}");
                }
            }

            return View(category);
        }

        // POST: /Admin/DeleteCategory/5
        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var category = _context.Categories.Find(id);
                if (category != null)
                {
                    _context.Categories.Remove(category);
                    _context.SaveChanges();
                }

                return RedirectToAction("ManageProducts");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
                return RedirectToAction("ManageProducts");
            }
        }
    }
}
