using LogisticWebApp.Models;

namespace LogisticWebApp.Data;

public static class DbSeed
{
    public static void Initialize(LogisticDbContext context)
    {
        // Assicurati che il database sia creato
        context.Database.EnsureCreated();

        // Se ci sono già dati nel database, non fare nulla
        if (context.Clienti.Any() || context.Corrieri.Any() || context.Spedizioni.Any())
        {
            return;
        }

        // Aggiungi clienti di esempio
        var clienti = new[]
        {
            new Cliente
            {
                Nome = "Mario",
                Cognome = "Rossi",
                Email = "mario.rossi@example.com"
            },
            new Cliente
            {
                Nome = "Giuseppe",
                Cognome = "Verdi",
                Email = "giuseppe.verdi@example.com"
            },
            new Cliente
            {
                Nome = "Anna",
                Cognome = "Bianchi",
                Email = "anna.bianchi@example.com"
            }
        };

        context.Clienti.AddRange(clienti);

        // Aggiungi corrieri di esempio
        var corrieri = new[]
        {
            new Corriere
            {
                Nome = "Luca",
                Cognome = "Ferrari",
                Email = "luca.ferrari@logistics.com",
                Telefono = "3331234567"
            },
            new Corriere
            {
                Nome = "Marco",
                Cognome = "Romano",
                Email = "marco.romano@logistics.com",
                Telefono = "3339876543"
            }
        };

        context.Corrieri.AddRange(corrieri);

        // Salva per ottenere gli ID
        context.SaveChanges();

        // Aggiungi spedizioni di esempio
        var spedizioni = new[]
        {
            new Spedizione
            {
                Cliente = clienti[0],
                Corriere = corrieri[0],
                Mittente = "Mario Rossi",
                Destinatario = "Paolo Neri",
                IndirizzoPartenza = "Via Roma 1, Milano",
                IndirizzoDestinazione = "Via Napoli 45, Roma",
                TipoMerce = "Elettronica",
                RichiesteSpeciali = "Fragile",
                Stato = "In preparazione",
                DataCreazione = DateTime.Now.AddDays(-2)
            },
            new Spedizione
            {
                Cliente = clienti[1],
                Corriere = corrieri[1],
                Mittente = "Giuseppe Verdi",
                Destinatario = "Laura Bianchi",
                IndirizzoPartenza = "Via Torino 23, Torino",
                IndirizzoDestinazione = "Via Palermo 12, Palermo",
                TipoMerce = "Abbigliamento",
                Stato = "In consegna",
                DataCreazione = DateTime.Now.AddDays(-1)
            },
            new Spedizione
            {
                Cliente = clienti[2],
                Corriere = corrieri[0],
                Mittente = "Anna Bianchi",
                Destinatario = "Marco Verdi",
                IndirizzoPartenza = "Via Firenze 56, Firenze",
                IndirizzoDestinazione = "Via Bologna 78, Bologna",
                TipoMerce = "Libri",
                Stato = "Consegnato",
                DataCreazione = DateTime.Now.AddDays(-3)
            }
        };

        context.Spedizioni.AddRange(spedizioni);
        context.SaveChanges();
    }
}