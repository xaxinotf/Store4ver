using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Store444.Contexts;
using Store444.Models;
using Store444.RepoInterfaces;

namespace Store444.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepo _orderRepo;

        public OrdersController(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepo.GetOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = await _orderRepo.GetUserOrdersAsync(userId);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> Create(int[] chosenDrugs)
        {
            var payments = await _orderRepo.GetPaymentsAsync();
            var shipTypes = await _orderRepo.GetShipTypesAsync();
            ViewData["PaymentType"] = payments.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            ViewData["ShipType"] = shipTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            TempData["ChosenDrugs"] = chosenDrugs;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var chosenDrugs = TempData["ChosenDrugs"] as int[];
            var products = await _orderRepo.GetProductsWithIdAsync(chosenDrugs);
            model.UserId = userId;
            var order = new Order()
            {
                Status = Status.Packaging,
                ShipTypeId = model.ShipTypeId,
                PaymentTypeId = model.PaymentTypeId,
                DeliveryAddress = model.DeliveryAddress,
                UserId = userId,
                Count = products.Count(),
                Price = products.Select(x=>x.Price).Sum()
            };

            await _orderRepo.OrderCreateAsync(order);
            foreach (var item in products)
            {
                order.Products.Add(item);
            }
            await _orderRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Details));

        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepo.GetOrderWithIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _orderRepo.GetOrderWithIdAsync(id);
            if (order != null)
            {
                await _orderRepo.DeleteAsync(order);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _orderRepo.GetOrderWithIdAsync(id);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Order model)
        {
            if (ModelState.IsValid)
            {
                await _orderRepo.EditAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
