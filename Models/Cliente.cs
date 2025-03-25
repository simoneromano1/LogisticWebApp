using System;

namespace LogisticWebApp.Models;

public class Cliente
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Cognome { get; set; }
    public required string Email { get; set; }

    // Navigation properties
    public virtual ICollection<Spedizione>? Spedizioni { get; set; }
    public virtual ICollection<Notifica>? Notifiche { get; set; }
}
