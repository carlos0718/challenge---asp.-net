using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestApp.Data;
using TestApp.Models;

namespace TestApp.Controllers
{
    public class SuscripcionsController : Controller
    {
        private readonly testDbContext _context;

        public SuscripcionsController(testDbContext context)
        {
            _context = context;
        }

        // GET: Suscripcions
        public async Task<IActionResult> Index()
        {
            var testDbContext = _context.Suscripcion.Include(s => s.Suscriptor);
            return View(await testDbContext.ToListAsync());
        }

        // GET: Suscripcions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Suscripcion == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripcion
                .Include(s => s.Suscriptor)
                .FirstOrDefaultAsync(m => m.IdAsociacion == id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            return View(suscripcion);
        }

        // GET: Suscripcions/Create
        public IActionResult Create()
        {
            ViewData["SuscriptorId"] = new SelectList(_context.Suscriptor, "SuscriptorId", "Apellido");
            return View();
        }

        // POST: Suscripcions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAsociacion,SuscriptorId,FechaAlta,FechaFin,MotivoFin")] Suscripcion suscripcion)
        {
            Suscriptor suscriptor = new Suscriptor();
            if (ModelState.IsValid)
            {
                suscripcion.FechaAlta = new DateTime().Date;
                _context.Add(suscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SuscriptorId"] = new SelectList(_context.Suscriptor, "SuscriptorId", "Apellido", suscripcion.SuscriptorId);
            return View("Index", suscriptor);
        }

        // GET: Suscripcions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Suscripcion == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripcion.FindAsync(id);
            if (suscripcion == null)
            {
                return NotFound();
            }
            ViewData["SuscriptorId"] = new SelectList(_context.Suscriptor, "SuscriptorId", "Apellido", suscripcion.SuscriptorId);
            return View(suscripcion);
        }

        // POST: Suscripcions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAsociacion,SuscriptorId,FechaAlta,FechaFin,MotivoFin")] Suscripcion suscripcion)
        {
            if (id != suscripcion.IdAsociacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suscripcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuscripcionExists(suscripcion.IdAsociacion))
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
            ViewData["SuscriptorId"] = new SelectList(_context.Suscriptor, "SuscriptorId", "Apellido", suscripcion.SuscriptorId);
            return View(suscripcion);
        }

        // GET: Suscripcions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Suscripcion == null)
            {
                return NotFound();
            }

            var suscripcion = await _context.Suscripcion
                .Include(s => s.Suscriptor)
                .FirstOrDefaultAsync(m => m.IdAsociacion == id);
            if (suscripcion == null)
            {
                return NotFound();
            }

            return View(suscripcion);
        }

        // POST: Suscripcions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Suscripcion == null)
            {
                return Problem("Entity set 'testDbContext.Suscripcion'  is null.");
            }
            var suscripcion = await _context.Suscripcion.FindAsync(id);
            if (suscripcion != null)
            {
                _context.Suscripcion.Remove(suscripcion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuscripcionExists(int id)
        {
          return _context.Suscripcion.Any(e => e.IdAsociacion == id);
        }
    }
}
