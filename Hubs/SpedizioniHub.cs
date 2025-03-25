using Microsoft.AspNetCore.SignalR;
using LogisticWebApp.Models;

namespace LogisticWebApp.Hubs;

public class SpedizioniHub : Hub
{
    public async Task InviaAggiornamentoSpedizione(Spedizione spedizione)
    {
        await Clients.All.SendAsync("RiceviAggiornamentoSpedizione", spedizione);
    }
}