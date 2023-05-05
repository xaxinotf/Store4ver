using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Store444.Contexts;
using Store444.Models;
using Store444.RepoInterfaces;

namespace Store444.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProdRepo _prodRepo;

        public ProductsController(IProdRepo prodRepo)
        {
            _prodRepo = prodRepo;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _prodRepo.GetProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _prodRepo.GetProductWithIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,RelaiseFromAndDosing,ShelfLife, Price")] Product product)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            product.UserId = userId;
            if (ModelState.IsValid)
            {
                await _prodRepo.CreateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _prodRepo.GetProductWithIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,RelaiseFromAndDosing,ShelfLife")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _prodRepo.EditProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _prodRepo.GetProductWithIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _prodRepo.GetProductWithIdAsync(id);
            if (product != null)
            {
                await _prodRepo.DeleteProductAsync(product);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Import()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProductExcel(IFormFile fileExcel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (fileExcel != null)
            {
                using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                {
                    await fileExcel.CopyToAsync(stream);
                    using (var workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                    {
                        foreach (var worksheet in workBook.Worksheets)
                        {
                            var c = worksheet.RowsUsed().Count();
                            foreach (var row in worksheet.RowsUsed())
                            {
                                try
                                {
                                    var product = new Product();
                                    product.Name = row.Cell(1).Value.ToString();
                                    product.RelaiseFromAndDosing = row.Cell(2).Value.ToString();
                                    product.ShelfLife = row.Cell(3).Value.ToString();
                                    await _prodRepo.CreateProductAsync(product);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Products");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Export()
        {
            int rowCount = 1;
            using (var workBook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var exexProducts = await _prodRepo.GetProductsAsync();
                var worksheet = workBook.Worksheets.Add("All products");
                foreach (var product in exexProducts)
                {
                    worksheet.Cell(rowCount, 1).Value = product.Name;
                    worksheet.Cell(rowCount, 2).Value = product.RelaiseFromAndDosing;
                    worksheet.Cell(rowCount, 3).Value = product.ShelfLife;
                    rowCount++;
                }
                using (var stream = new MemoryStream())
                {
                    workBook.SaveAs(stream);
                    await stream.FlushAsync();
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"ShipperProducts_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        public async Task<IActionResult> ShipperProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shipperProds = await _prodRepo.GetShipperProductsAsync(userId);
            return View(shipperProds);
        }
    }
}
