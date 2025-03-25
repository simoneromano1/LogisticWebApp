using System;

namespace LogisticWebApp.Models;

public class Notifica
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int SpedizioneId { get; set; }
    public required string Messaggio { get; set; }
    public DateTime DataNotifica { get; set; }
    public required string StatoNotifica { get; set; }

    // Navigation properties
    public required virtual Cliente Cliente { get; set; }
    public required virtual Spedizione Spedizione { get; set; }
}
