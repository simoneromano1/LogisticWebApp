using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using LogisticWebApp.Data;
using LogisticWebApp.Models;
using LogisticWebApp.Hubs;
using LogisticWebApp.Services;
using Microsoft.AspNetCore.SignalR;

namespace LogisticWebApp.Controllers;

public class SpedizioniController : Controller
{
    private readonly LogisticDbContext _context;
    private readonly IHubContext<SpedizioniHub> _hubContext;
    private readonly IEmailService _emailService;

    public SpedizioniController(LogisticDbContext context, IHubContext<SpedizioniHub> hubContext, IEmailService emailService)
    {
        _context = context;
        _hubContext = hubContext;
        _emailService = emailService;
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

            // Invia email e notifica i client della nuova spedizione
            await NotificaStatoSpedizione(spedizione.Id, spedizione.Stato);

            return RedirectToAction(nameof(Index));
        }
        ViewData["IdCliente"] = new SelectList(_context.Clienti, "Id", "Nome");
        ViewData["IdCorriere"] = new SelectList(_context.Corrieri, "Id", "Nome");
        return View(spedizione);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var spedizione = await _context.Spedizioni
            .Include(s => s.Cliente)
            .Include(s => s.Corriere)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (spedizione == null)
            return NotFound();

        ViewData["IdCliente"] = new SelectList(_context.Clienti, "Id", "Nome", spedizione.IdCliente);
        ViewData["IdCorriere"] = new SelectList(_context.Corrieri, "Id", "Nome", spedizione.IdCorriere);
        return View(spedizione);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,IdCliente,IdCorriere,Mittente,Destinatario,IndirizzoPartenza,IndirizzoDestinazione,TipoMerce,RichiesteSpeciali,Stato,DataCreazione")] Spedizione spedizione)
    {
        if (id != spedizione.Id)
            return NotFound();

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
            try
            {
                // Ottieni lo stato precedente
                var spedizioneDb = await _context.Spedizioni.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
                var statoPrec = spedizioneDb?.Stato;

                _context.Update(spedizione);
                await _context.SaveChangesAsync();

                // Se lo stato è cambiato, invia email e notifica
                if (statoPrec != spedizione.Stato)
                {
                    await NotificaStatoSpedizione(spedizione.Id, spedizione.Stato);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpedizioneExists(spedizione.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        ViewData["IdCliente"] = new SelectList(_context.Clienti, "Id", "Nome", spedizione.IdCliente);
        ViewData["IdCorriere"] = new SelectList(_context.Corrieri, "Id", "Nome", spedizione.IdCorriere);
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

    private async Task NotificaStatoSpedizione(int id, string nuovoStato)
    {
        var spedizione = await _context.Spedizioni
            .Include(s => s.Cliente)
            .Include(s => s.Corriere)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (spedizione == null)
            return;

        // Invia email al cliente
        if (spedizione.Cliente?.Email != null)
        {
            var subject = $"Aggiornamento stato spedizione #{spedizione.Id}";
            var body = $@"
                <h2>Aggiornamento stato spedizione</h2>
                <p>Gentile {spedizione.Cliente.Nome},</p>
                <p>La sua spedizione #{spedizione.Id} è stata aggiornata.</p>
                <p>Nuovo stato: <strong>{nuovoStato}</strong></p>
                <p>Dettagli spedizione:</p>
                <ul>
                    <li>Da: {spedizione.Mittente}</li>
                    <li>A: {spedizione.Destinatario}</li>
                    <li>Indirizzo di consegna: {spedizione.IndirizzoDestinazione}</li>
                </ul>
                <p>Cordiali saluti,<br>LogisticApp</p>";

            await _emailService.SendEmailAsync(spedizione.Cliente.Email, subject, body);
        }

        // Notifica i client dell'aggiornamento
        await _hubContext.Clients.All.SendAsync("RiceviAggiornamentoSpedizione", spedizione);
    }

    private bool SpedizioneExists(int id)
    {
        return _context.Spedizioni.Any(e => e.Id == id);
    }
}