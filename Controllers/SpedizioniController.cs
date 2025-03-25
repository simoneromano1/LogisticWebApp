using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using LogisticWebApp.Data;
using LogisticWebApp.Models;

namespace LogisticWebApp.Controllers;

public class SpedizioniController : Controller
{
    private readonly LogisticDbContext _context;

    public SpedizioniController(LogisticDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Spedizioni
            .Include(s => s.Cliente)
            .Include(s => s.Corriere)
            .ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var spedizione = await _context.Spedizioni
            .Include(s => s.Cliente)
            .Include(s => s.Corriere)
            .Include(s => s.Feedback)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (spedizione == null)
            return NotFound();

        return View(spedizione);
    }

    public IActionResult Create()
    {
        ViewData["ClienteId"] = new SelectList(_context.Clienti, "Id", "Nome");
        ViewData["CorriereId"] = new SelectList(_context.Corrieri, "Id", "Nome");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ClienteId,CorriereId,Mittente,Destinatario,IndirizzoPartenza,IndirizzoDestinazione,TipoMerce,RichiesteSpeciali,Stato,DataCreazione")] Spedizione spedizione)
    {
        if (ModelState.IsValid)
        {
            spedizione.DataCreazione = DateTime.Now;
            spedizione.Stato = "In preparazione";
            _context.Add(spedizione);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClienteId"] = new SelectList(_context.Clienti, "Id", "Nome");
        ViewData["CorriereId"] = new SelectList(_context.Corrieri, "Id", "Nome");
        return View(spedizione);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var spedizione = await _context.Spedizioni
            .Include(s => s.Cliente)
            .Include(s => s.Corriere)
            .Include(s => s.Feedback)
            .Include(s => s.Notifiche)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (spedizione == null)
            return NotFound();

        return View(spedizione);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var spedizione = await _context.Spedizioni
            .Include(s => s.Feedback)
            .Include(s => s.Notifiche)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (spedizione != null)
        {
            _context.Spedizioni.Remove(spedizione);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool SpedizioneExists(int id)
    {
        return _context.Spedizioni.Any(e => e.Id == id);
    }
}