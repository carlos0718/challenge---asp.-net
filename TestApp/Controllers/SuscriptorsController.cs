using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Versioning;
using TestApp.Data;
using TestApp.Models;
using TestApp.Security;

namespace TestApp.Controllers
{
    public class SuscriptorsController : Controller
    {
        private readonly testDbContext _context;

        public SuscriptorsController(testDbContext context)
        {
            _context = context;
        }

        // GET: Suscriptors
        public IActionResult Index(string tipo, string numero, bool nuevo) {
            ViewData["Tipo"] = new List<string>() { "DNI", "PASAPORTE" };
            ViewData["Alert"] = "";

            if (!String.IsNullOrEmpty(tipo) && !String.IsNullOrEmpty(numero)) {
                #region Busqueda de suscriptor
                var result = from s in _context.Suscriptor
                             where s.NumeroDocumento == numero && s.TipoDocumento == tipo
                             select s;
     

                if (result.IsNullOrEmpty()) {
                    ViewData["Alert"] = "Usuario no existe";
                } else {
                    #endregion

                    #region Validación de suscripción
                    var subs = from s in _context.Suscripcion
                                                  select s;

                    string state = "No suscripto";

                    foreach (var item in subs) {
                        foreach (var sus in result){
                            if(item.SuscriptorId == sus.SuscriptorId){
                                state = "Suscripto";
                            }
                        }
                    }

                    ViewData["Estado"] = state;
                    #endregion

                }

                return View(result?.ToList());
            }

            #region Habilita inputs para nuevo suscriptor
            bool btnNuevo = true;
            if (nuevo) {
                btnNuevo = false;
            }
            ViewData["Nuevo"] = btnNuevo;
            #endregion

            return View();
        }

        // GET: Suscriptors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Suscriptor == null)
            {
                return NotFound();
            }

            var suscriptor = await _context.Suscriptor
                .FirstOrDefaultAsync(m => m.SuscriptorId == id);
            if (suscriptor == null)
            {
                return NotFound();
            }

            return View(suscriptor);
        }

        // GET: Suscriptors/Create
        public IActionResult Create()
        {
            ViewData["Tipo"] = new List<string>() { "DNI", "PASAPORTE" };
            return View();
        }

        // POST: Suscriptors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string TipoDocumento, [Bind("SuscriptorId,Nombre,Apellido,NumeroDocumento,TipoDocumento,Direccion,Email,NombreUsuario,PasswordUsuario")] Suscriptor suscriptor)
        {
            ViewData["Tipo"] = new List<string>() { "DNI", "PASAPORTE" };
            if (ModelState.IsValid)
            {
                suscriptor.TipoDocumento= TipoDocumento;
                suscriptor.PasswordUsuario = Encrypt.GetSHA256(suscriptor.PasswordUsuario);
                _context.Add(suscriptor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(suscriptor);
        }

        // GET: Suscriptors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Tipo"] = new List<string>() { "DNI", "PASAPORTE" };
            if (id == null || _context.Suscriptor == null)
            {
                return NotFound();
            }

            var suscriptor = await _context.Suscriptor.FindAsync(id);
            if (suscriptor == null)
            {
                return NotFound();
            }
            return View(suscriptor);
        }

        // POST: Suscriptors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string TipoDocumento, int id, [Bind("SuscriptorId,Nombre,Apellido,NumeroDocumento,TipoDocumento,Direccion,Email,NombreUsuario,PasswordUsuario")] Suscriptor suscriptor)
        {
            ViewData["Tipo"] = new List<string>() { "DNI", "PASAPORTE" };

            if (id != suscriptor.SuscriptorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    suscriptor.TipoDocumento = TipoDocumento;
                    suscriptor.PasswordUsuario = Encrypt.GetSHA256(suscriptor.PasswordUsuario);
                    _context.Update(suscriptor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuscriptorExists(suscriptor.SuscriptorId))
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
            return View(suscriptor);
        }

        private bool SuscriptorExists(int id) {
            return _context.Suscriptor.Any(e => e.SuscriptorId == id);
        }

        // GET: Suscriptors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Suscriptor == null)
            {
                return NotFound();
            }

            var suscriptor = await _context.Suscriptor
                .FirstOrDefaultAsync(m => m.SuscriptorId == id);
            if (suscriptor == null)
            {
                return NotFound();
            }

            return View(suscriptor);
        }

        // POST: Suscriptors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Suscriptor == null)
            {
                return Problem("Entity set 'testDbContext.Suscriptor'  is null.");
            }
            var suscriptor = await _context.Suscriptor.FindAsync(id);
            if (suscriptor != null)
            {
                _context.Suscriptor.Remove(suscriptor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Suscriptors/Edit/5
        public async Task<IActionResult> Suscription(int? id) {
            ViewData["Tipo"] = new List<string>() { "DNI", "PASAPORTE" };
            if (id == null || _context.Suscriptor == null) {
                return NotFound();
            }

            var suscriptor = await _context.Suscriptor.FindAsync(id);
            if (suscriptor == null) {
                return NotFound();
            }
            return View(suscriptor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Suscription(int id, [Bind("SuscriptorId,Nombre,Apellido,NumeroDocumento,TipoDocumento,Direccion,Email,NombreUsuario,PasswordUsuario")] Suscriptor suscriptor) {
            ViewData["Tipo"] = new List<string>() { "DNI", "PASAPORTE" };
            Suscripcion suscripcion = new Suscripcion();
            if (!ModelState.IsValid) {
                suscripcion.SuscriptorId = id;
                suscripcion.FechaAlta = DateTime.Today;
                _context.Suscripcion.Add(suscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index");
        }


    }
}
