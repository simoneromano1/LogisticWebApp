using System;

namespace LogisticWebApp.Models;

public class Feedback
{
    public int Id { get; set; }
    public int SpedizioneId { get; set; }
    public int Valutazione { get; set; }
    public string? Commento { get; set; }
    public DateTime DataFeedback { get; set; }

    // Navigation property
    public required virtual Spedizione Spedizione { get; set; }
}
