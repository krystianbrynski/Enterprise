using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcNews.Data;
using MvcNews.Models;
using Newtonsoft.Json.Linq;

namespace MvcNews.Controllers
{
    public class NewsItemsController : Controller
    {
        private readonly NewsDbContext _context;

        public NewsItemsController(NewsDbContext context)
        {
            _context = context;
        }

        // GET: NewsItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.News.ToListAsync());
        }

        // GET: NewsItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.News
                .FirstOrDefaultAsync(m => m.ID == id);
            if (newsItem == null)
            {
                return NotFound();
            }

            return View(newsItem);
        }

        // GET: NewsItems/Create
        public IActionResult Create()
        {
            var newsItem = new NewsItem
            {
                Timestamp = System.DateTime.Now
            };
            return View(newsItem);
        }

        // POST: NewsItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Timestamp,Text,RowVersion")] NewsItem newsItem)
        {
            try
            {
                _context.Add(newsItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.GetBaseException().Message);
                return View(newsItem);
            }
        }

        // GET: NewsItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.News.FindAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            return View(newsItem);
        }

        // POST: NewsItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("ID,Timestamp,Text,RowVersion")] NewsItem newsItem)
{
    if (id != newsItem.ID)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            _context.Update(newsItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException e)
        {
            if (!NewsItemExists(newsItem.ID))
            {
                ModelState.AddModelError(string.Empty,
                    "Wpis został usunięty przez innego użytkownika.");
            }
            else
            {
                ModelState.AddModelError(string.Empty,
                    "Wpis został zmodyfikowany przez innego użytkownika. Twoje zmiany zostały odrzucone.");

                // odczyt aktualnych danych z bazy
                var entry = e.Entries.Single();
                var databaseEntry = entry.GetDatabaseValues();
                var databaseEntity = (NewsItem)databaseEntry.ToObject();

                // podmiana wartości właściwości wersji w edytowanej instancji
                newsItem.RowVersion = (byte[])databaseEntity.RowVersion;
                ModelState.Remove("RowVersion");

                // przekazanie informacji o aktualnych wartościach atrybutów
                ModelState.AddModelError("Timestamp", "Current value: "
                    + ((DateTime)databaseEntity.Timestamp).ToString("g"));
                ModelState.AddModelError("Text", "Current value: "
                    + (string)databaseEntity.Text);
            }
        }
    }

    return View(newsItem);
}

        // GET: NewsItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.News
                .FirstOrDefaultAsync(m => m.ID == id);
            if (newsItem == null)
            {
                return NotFound();
            }

            return View(newsItem);
        }

        // POST: NewsItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [Bind("ID,RowVersion")] NewsItem newsItem)
        {
            try
            {
                _context.Entry(newsItem).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!NewsItemExists(newsItem.ID))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. The record was modified by another user after you got the original value");


                    var entry = e.Entries.Single();
                    var databaseEntry = entry.GetDatabaseValues();
                    var databaseEntity = (NewsItem)databaseEntry.ToObject();
                    ModelState.Remove("RowVersion");
                    return View(databaseEntity);
                }
              }
            }

        private bool NewsItemExists(int id)
        {
            return _context.News.Any(e => e.ID == id);
        }
    }
}
