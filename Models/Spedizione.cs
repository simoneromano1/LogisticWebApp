using System;

namespace LogisticWebApp.Models;

public class Spedizione
{
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public int IdCorriere { get; set; }
    public required string Mittente { get; set; }
    public required string Destinatario { get; set; }
    public required string IndirizzoPartenza { get; set; }
    public required string IndirizzoDestinazione { get; set; }
    public required string TipoMerce { get; set; }
    public string? RichiesteSpeciali { get; set; }
    public required string Stato { get; set; }
    public DateTime DataCreazione { get; set; }

    // Navigation properties
    public virtual Cliente? Cliente { get; set; }
    public virtual Corriere? Corriere { get; set; }
    public virtual Feedback? Feedback { get; set; }
    public virtual ICollection<Notifica>? Notifiche { get; set; }
}
