using C2H.Data.Services;
using C2H.Models;
using C2H.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace C2H.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.getProducts().Where(x => x.Name.StartsWith("A"));
            if (products.Count() == 0)
                _productService.populateRandomProducts();

            var productList = new List<Product>();
            foreach (var product in products)
            {
                productList.Add(new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                });
            }

            return View(productList.Take(10));
        }

        public IActionResult FilterByLetter(string data)
        {
            var products = _productService.getProducts().Where(x=>x.Name.StartsWith(data));
            var productList = new List<Product>();
            foreach (var product in products)
            {
                productList.Add(new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                });
            }

            return PartialView("_Products", productList);
        }

        public IActionResult Search(string data)
        {
            if (string.IsNullOrEmpty(data))
                return RedirectToAction("FilterByLetter", new {data = "A"});

            var products = _productService.getProducts().Where(x => x.Name.Contains(data));
            var productList = new List<Product>();
            foreach (var product in products)
            {
                productList.Add(new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                });
            }

            return PartialView("_Products", productList);
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
}