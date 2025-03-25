using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticWebApp.Data;
using LogisticWebApp.Models;

namespace LogisticWebApp.Controllers;

public class ClientiController : Controller
{
    private readonly LogisticDbContext _context;

    public ClientiController(LogisticDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Clienti.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var cliente = await _context.Clienti
            .Include(c => c.Spedizioni)
            .Include(c => c.Notifiche)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (cliente == null)
            return NotFound();

        return View(cliente);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Cognome,Email")] Cliente cliente)
    {
        if (ModelState.IsValid)
        {
            _context.Add(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var cliente = await _context.Clienti.FindAsync(id);
        if (cliente == null)
            return NotFound();

        return View(cliente);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cognome,Email")] Cliente cliente)
    {
        if (id != cliente.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(cliente.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var cliente = await _context.Clienti
            .Include(c => c.Spedizioni)
            .Include(c => c.Notifiche)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (cliente == null)
            return NotFound();

        return View(cliente);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cliente = await _context.Clienti
            .Include(c => c.Spedizioni)
            .Include(c => c.Notifiche)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (cliente != null)
        {
            _context.Clienti.Remove(cliente);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ClienteExists(int id)
    {
        return _context.Clienti.Any(e => e.Id == id);
    }
}
