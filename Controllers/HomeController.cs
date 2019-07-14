using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsList.Models;

namespace ProductsList.Controllers
{
    public class HomeController : Controller
    {
        protected ProductsDbContext _pContext = null;
        public HomeController(ProductsDbContext pContext)
        {
            _pContext = pContext;
        }
        public IActionResult Index()
        {
            var products = _pContext.Products.Include(p => p.Category)
                .Select(p => new ProductListModel()
                {
                    Id = p.Id,
                    Code = p.Code,
                    Category = p.Category.Name,
                    IsActive = p.IsActive.GetValueOrDefault(),
                    Price = p.Price,
                    Name = p.Name
                }).ToArray();
            return View(products);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Settings()
        {
            var categories = _pContext.Categories.ToArray();
            return View(categories);
        }

        public async Task<IActionResult> SaveCategory(string name, int id = 0)
        {
            Categories category = null;
            if (id == 0)
            {
                category = new Categories()
                {
                    Name = name
                };
                _pContext.Categories.Add(category);
            }
            else
            {
                category = _pContext.Categories.FirstOrDefault(f => f.Id == id);
                if (category != null)
                {
                    category.Name = name;
                }
            }
            await _pContext.SaveChangesAsync();
            return Json(category);
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = _pContext.Categories.FirstOrDefault(f => f.Id == id);
            if (category != null)
            {
                _pContext.Categories.Remove(category);
                await _pContext.SaveChangesAsync();
            }
            return Json(category);
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _pContext.Products.FirstOrDefault(f => f.Id == id);
            if (product != null)
            {
                _pContext.Products.Remove(product);
                await _pContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditProduct(int? id = 0)
        {
            var category = _pContext.Categories.FirstOrDefault();
            var product = _pContext.Products.FirstOrDefault(f => f.Id == id) ??
                          new Products() { IsActive = true, CategoryId = (category?.Id).GetValueOrDefault() };
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Products model)
        {
            if (!ModelState.IsValid)
            {
                this.ViewBag.Message = "Fill in all the fields.";
                return View(model);
            }
            if (model.Id == 0)
            {
                model.Updated = DateTime.Now;
                _pContext.Products.Add(model);
                await _pContext.SaveChangesAsync();
                this.ViewBag.Message = "Product successfully saved.";
                return RedirectToAction("EditProduct", new {id = model.Id});
            }
            else
            {
                var product = _pContext.Products.FirstOrDefault(p => p.Id == model.Id);
                product.Updated = DateTime.Now;
                product.IsActive = model.IsActive;
                product.Code = model.Code;
                product.Name = model.Name;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                this.ViewBag.Message = "Product successfully saved.";
                await _pContext.SaveChangesAsync();
                return View(model);
            }
        }

        public IActionResult GetCategories()
        {
            return Json(_pContext.Categories.Select(s => new
            {
                s.Id,
                s.Name
            }).ToArray());
        }
    }
}
