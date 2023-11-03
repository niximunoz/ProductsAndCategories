using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAndCategories.Models;

namespace ProductsAndCategories.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        List<Product> allProducts = _context.Products.ToList();
        ViewBag.AllProducts = allProducts;
        return View();
    }

    [HttpGet("categories")]
    public IActionResult Categories()
    {
        List<Category> allCategories = _context.Categories.ToList();
        ViewBag.AllCategories = allCategories;
        return View("Categories");
    }
    [HttpPost("new/product")]
    public IActionResult NewProduct(Product newProduct)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View("Index");
    }

    [HttpPost("new/category")]
    public IActionResult NewCategory(Category newCategory)
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("Categories");
        }
        return View("Categories");
    }

    [HttpGet("products/{id_product}")]

    public IActionResult Product(int id_product)
    {
        Product? product = _context.Products
            .Include(p => p.Associations)
            .ThenInclude(prod => prod.Category)
            .FirstOrDefault(p => p.ProductId == id_product);
        ViewBag.Product = product;
        var allCategories = _context.Categories.ToList();
        var selectedCategories = product.Associations.Select(a => a.Category).ToList();
        var unselectedCategories = allCategories.Except(selectedCategories).ToList();
        ViewBag.Categories = selectedCategories;
        ViewBag.AllCategories = unselectedCategories;
        return View("Product");
    }
    [HttpPost("add/category")]

    public IActionResult AddCategory(Association NewAssociation)
    {
        _context.Associations.Add(NewAssociation);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
