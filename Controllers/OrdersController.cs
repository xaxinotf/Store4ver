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

namespace Store444.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DrugShopContext _context;

        public OrdersController(DrugShopContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var drugShopContext = _context.Orders.Include(o => o.PaymentType).Include(o => o.ShipTypeNavigation);
            return View(await drugShopContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (id == null || _context.Orders == null)
            //{
            //    return NotFound();
            //}

            var order = await _context.Orders
                .Include(o => o.PaymentType)
                .Include(o => o.ShipTypeNavigation)
                .Where(x=>x.UserId == userId)
                .ToListAsync();
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        public async Task<IActionResult> Create()
        {
            var payments = await _context.PaymentTypes.ToListAsync();
            var shipTypes = await _context.ShipTypes.ToListAsync();
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Name", payments);
            ViewData["ShipType"] = new SelectList(_context.ShipTypes, "Id", "Name", shipTypes);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,ShipType,PaymentTypeId,UserId,DeliveryAddress")] Order model, int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            model.UserId = userId;
            var order = new Order()
            {
                ShipType = model.ShipType,
                PaymentType = model.PaymentType,
                DeliveryAddress = model.DeliveryAddress,
                UserId = userId
            };

            _context.Orders.Add(order);
            order.OrderProducts.Add(new OrderProduct { Count = 1, Price = 1, Product = product });
            _context.SaveChanges();
            return RedirectToAction(nameof(Details));

        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Id", order.PaymentTypeId);
            ViewData["ShipType"] = new SelectList(_context.ShipTypes, "Id", "Id", order.ShipType);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,ShipType,PaymentTypeId,UserId,DeliveryAddress")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PaymentTypeId"] = new SelectList(_context.PaymentTypes, "Id", "Id", order.PaymentTypeId);
            ViewData["ShipType"] = new SelectList(_context.ShipTypes, "Id", "Id", order.ShipType);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.PaymentType)
                .Include(o => o.ShipTypeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'DrugShopContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
