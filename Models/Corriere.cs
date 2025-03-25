using System;

namespace LogisticWebApp.Models;

public class Corriere
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Cognome { get; set; }
    public required string Telefono { get; set; }
    public required string Email { get; set; }

    // Navigation property
    public virtual ICollection<Spedizione>? Spedizioni { get; set; }
}
