﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaryGillespie_Practical.Data;
using GaryGillespie_Practical.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GaryGillespie_Practical.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Dependency inject db context + user manager for unit testing
        /// </summary>
        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = _context.Products
                .Include(p => p.Log); //eager load operations log
            return View(await products.ToListAsync());
        }

        /// <summary>
        /// Action result used by datatables to load all products and lazy (eagerly) load
        /// the related operations log, and related operation log application user
        /// </summary>
        /// <returns></returns>
        public IActionResult LoadAllProducts()
        {
            var prodcuts = _context.Products
                .Include(product => product.Log)
                .ThenInclude(log => log.User)
                .ToList();

            var data = prodcuts;
            return Json(new { data });
        }
                
        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["LogId"] = new SelectList(_context.Logs, "Id", "UserId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,PurchasePrice,SalesPrice,Quantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                //Get Current User Id
                System.Security.Claims.ClaimsPrincipal user = User;
                var userId = _userManager.GetUserId(User);

                //Create new operation Log instance
                OperationLog log = new OperationLog { UserId = userId, CreatedDate = DateTime.Now };

                //map operation log to product
                product.LogId = log.Id;
                product.Log = log;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LogId"] = new SelectList(_context.Logs, "Id", "UserId", product.LogId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["LogId"] = new SelectList(_context.Logs, "Id", "UserId", product.LogId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,PurchasePrice,SalesPrice,Quantity,LogId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if(product.Log == null)
            {
                product.Log = _context.Logs.SingleOrDefault(l => l.Id == product.LogId);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["LogId"] = new SelectList(_context.Logs, "Id", "UserId", product.LogId);
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed([FromBody] int productId)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == productId);
            _context.Products.Remove(product);
            _context.SaveChanges();
            var response = "success";
            return Json(response);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
