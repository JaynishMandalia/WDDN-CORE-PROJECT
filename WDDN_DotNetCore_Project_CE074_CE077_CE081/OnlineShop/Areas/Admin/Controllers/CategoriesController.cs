using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Infrastructure;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    //[Authorize(Roles = "admin")]
    [Area("admin")]
    public class CategoriesController : Controller
    {
        private readonly OnlineShopContext context;
        public CategoriesController(OnlineShopContext context)
        {
            this.context = context;
        }

        //GET admin/categories
        public async Task<IActionResult> Index()
        {
            return View(await context.Categories.OrderBy(x => x.Sorting).ToListAsync());
        }

        //GET admin/categories/create
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        //POST request   path => /admin/categories/create
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 100;
                var slug = await context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Category Already Exists.");
                    return View(category);
                }
                context.Add(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "The Category has been added!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //GET request   path => /admin/categories/edit/5
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ValidateAntiForgeryToken]
        //POST request   path => /admin/categories/edit/5
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");

                var slug = await context.Categories.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "Category Already Exists.");
                    return View(category);
                }
                context.Update(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "The Category has been edited!";
                return RedirectToAction("Edit", new { id });
            }
            return View(category);
        }

        //GET request   path => /admin/categories/delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "The category doest not exists!!";
            }
            else
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "The category has been deleted successfully!";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        //POST request   path => /admin/categories/reorder
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;
            foreach (var categoryId in id)
            {
                Category category = await context.Categories.FindAsync(categoryId);
                category.Sorting = count;
                context.Update(category);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }
    }
}
