using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using LogisticWebApp.Data;
using LogisticWebApp.Models;
using LogisticWebApp.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace LogisticWebApp.Controllers;

public class SpedizioniController : Controller
{
    private readonly LogisticDbContext _context;
    private readonly IHubContext<SpedizioniHub> _hubContext;

    public SpedizioniController(LogisticDbContext context, IHubContext<SpedizioniHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
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
        ViewData["IdCliente"] = new SelectList(_context.Clienti, "Id", "Nome");
        ViewData["IdCorriere"] = new SelectList(_context.Corrieri, "Id", "Nome");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,IdCliente,IdCorriere,Mittente,Destinatario,IndirizzoPartenza,IndirizzoDestinazione,TipoMerce,RichiesteSpeciali,Stato,DataCreazione")] Spedizione spedizione)
    {
        var cliente = await _context.Clienti.FindAsync(spedizione.IdCliente);
        var corriere = await _context.Corrieri.FindAsync(spedizione.IdCorriere);

        if (cliente == null)
        {
            ModelState.AddModelError("IdCliente", "Cliente non trovato");
        }
        else
        {
            spedizione.Cliente = cliente;
        }

        if (corriere == null)
        {
            ModelState.AddModelError("IdCorriere", "Corriere non trovato");
        }
        else
        {
            spedizione.Corriere = corriere;
        }

        if (ModelState.IsValid)
        {
            spedizione.DataCreazione = DateTime.Now;
            spedizione.Stato = "In preparazione";
            _context.Add(spedizione);
            await _context.SaveChangesAsync();

            // Notifica i client della nuova spedizione
            var spedizioneCompleta = await _context.Spedizioni
                .Include(s => s.Cliente)
                .Include(s => s.Corriere)
                .FirstOrDefaultAsync(s => s.Id == spedizione.Id);
            await _hubContext.Clients.All.SendAsync("RiceviAggiornamentoSpedizione", spedizioneCompleta);

            return RedirectToAction(nameof(Index));
        }
        ViewData["IdCliente"] = new SelectList(_context.Clienti, "Id", "Nome");
        ViewData["IdCorriere"] = new SelectList(_context.Corrieri, "Id", "Nome");
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStato(int id, string nuovoStato)
    {
        var spedizione = await _context.Spedizioni
            .Include(s => s.Cliente)
            .Include(s => s.Corriere)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (spedizione == null)
            return NotFound();

        spedizione.Stato = nuovoStato;
        await _context.SaveChangesAsync();

        // Notifica i client dell'aggiornamento
        await _hubContext.Clients.All.SendAsync("RiceviAggiornamentoSpedizione", spedizione);

        return Ok();
    }

    private bool SpedizioneExists(int id)
    {
        return _context.Spedizioni.Any(e => e.Id == id);
    }
}