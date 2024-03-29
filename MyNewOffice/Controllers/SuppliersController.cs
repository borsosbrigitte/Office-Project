﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeModel.Data;
using OfficeModel.Models;
using OfficeModel.Models.OfficeViewModels;

namespace Office.Controllers
{
    [Authorize(Policy = "OnlySales")]
    public class SuppliersController : Controller
    {
        private readonly OfficeContext _context;

        public SuppliersController(OfficeContext context)
        {
            _context = context;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index(int? id, int? productID)
        {
            var viewModel = new SupplierIndexData();
            viewModel.Suppliers = await _context.Suppliers
                .Include(i => i.SuppliedProducts).ThenInclude(i => i.Product).ThenInclude(i => i.Orders).ThenInclude(i => i.Client).AsNoTracking().OrderBy(i => i.SupplierName).ToListAsync();
            if (id != null)
            {
                ViewData["SupplierID"] = id.Value;
                Supplier supplier = viewModel.Suppliers.Where(
                i => i.SupplierID == id.Value).Single();
                viewModel.Products = supplier.SuppliedProducts.Select(s => s.Product);
            }
            if (productID != null)
            {
                ViewData["ProductID"] = productID.Value;
                viewModel.Orders = viewModel.Products.Where(
                x => x.ProductID == productID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierID,SupplierName,Address")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var supplier = await _context.Suppliers
            .Include(i => i.SuppliedProducts).ThenInclude(i => i.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.SupplierID == id);
            if (supplier == null)
            {
                return NotFound();
            }
            PopulateSuppliedProductData(supplier);
            return View(supplier);
        }
        private void PopulateSuppliedProductData(Supplier supplier)
        {
            var allProducts = _context.Products;
            var supplierProducts = new HashSet<int>(supplier.SuppliedProducts.Select(c => c.ProductID));
            var viewModel = new List<SuppliedProductData>();
            foreach (var product in allProducts)
            {
                viewModel.Add(new SuppliedProductData
                {
                    ProductID = product.ProductID,
                    Name = product.Name,
                    IsSupplied = supplierProducts.Contains(product.ProductID)
                });
            }
            ViewData["Products"] = viewModel;



        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedProducts)
        {
            if (id == null)
            {
                return NotFound();
            }
            var supplierToUpdate = await _context.Suppliers
            .Include(i => i.SuppliedProducts)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(m => m.SupplierID == id);
            if (await TryUpdateModelAsync<Supplier>(
            supplierToUpdate,
            "",
            i => i.SupplierName, i => i.Address))
            {
                UpdateSuppliedProducts(selectedProducts, supplierToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateSuppliedProducts(selectedProducts, supplierToUpdate);
            PopulateSuppliedProductData(supplierToUpdate);
            return View(supplierToUpdate);
        }
        private void UpdateSuppliedProducts(string[] selectedProducts, Supplier supplierToUpdate)
        {
            if (selectedProducts == null)
            {
                supplierToUpdate.SuppliedProducts = new List<SuppliedProduct>();
                return;
            }
            var selectedProductsHS = new HashSet<string>(selectedProducts);
            var suppliedProducts = new HashSet<int>
            (supplierToUpdate.SuppliedProducts.Select(c => c.Product.ProductID));
            foreach (var product in _context.Products)
            {
                if (selectedProductsHS.Contains(product.ProductID.ToString()))
                {
                    if (!suppliedProducts.Contains(product.ProductID))
                    {
                        supplierToUpdate.SuppliedProducts.Add(new SuppliedProduct { SupplierID = supplierToUpdate.SupplierID, ProductID = product.ProductID });
                    }
                }
                else
                {
                    if (suppliedProducts.Contains(product.ProductID))
                    {
                        SuppliedProduct productToRemove = supplierToUpdate.SuppliedProducts.FirstOrDefault(i => i.ProductID == product.ProductID);
                        _context.Remove(productToRemove);
                    }
                }
            }
        }


        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.SupplierID == id);
        }
    }
}
