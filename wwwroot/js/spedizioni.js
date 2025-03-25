"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/spedizioniHub")
    .withAutomaticReconnect([0, 2000, 5000, 10000, 20000]) // Retry after 0s, 2s, 5s, 10s, then every 20s
    .configureLogging(signalR.LogLevel.Information)
    .build();

// Gestione dello stato della connessione
let statusDiv;

function updateConnectionStatus(status, message) {
    if (!statusDiv) {
        statusDiv = document.createElement('div');
        statusDiv.className = 'connection-status';
        document.querySelector('#spedizioni-table').parentNode.insertBefore(statusDiv, document.querySelector('#spedizioni-table'));
    }
    statusDiv.className = `connection-status ${status}`;
    statusDiv.textContent = message;
}

connection.onreconnecting(error => {
    updateConnectionStatus('reconnecting', 'Riconnessione in corso...');
    console.warn('Tentativo di riconnessione:', error);
});

connection.onreconnected(connectionId => {
    updateConnectionStatus('connected', 'Connesso');
    console.log('Riconnesso con ID:', connectionId);
    // Ricarica i dati per assicurarsi di avere lo stato più aggiornato
    location.reload();
});

connection.onclose(error => {
    updateConnectionStatus('disconnected', 'Disconnesso dal server');
    console.error('Connessione chiusa:', error);
});

// Gestione degli aggiornamenti delle spedizioni
connection.on("RiceviAggiornamentoSpedizione", function (spedizione) {
    const row = document.querySelector(`tr[data-spedizione-id="${spedizione.id}"]`);

    if (row) {
        updateSpedizioneRow(row, spedizione);
    } else {
        addSpedizioneRow(spedizione);
    }
});

function updateSpedizioneRow(row, spedizione) {
    row.querySelector('.cliente-nome').textContent = spedizione.cliente.nome;
    row.querySelector('.corriere-nome').textContent = spedizione.corriere.nome;
    row.querySelector('.mittente').textContent = spedizione.mittente;
    row.querySelector('.destinatario').textContent = spedizione.destinatario;
    row.querySelector('.stato').textContent = spedizione.stato;

    // Evidenzia brevemente la riga aggiornata
    row.classList.add('updated');
    setTimeout(() => row.classList.remove('updated'), 2000);
}

function addSpedizioneRow(spedizione) {
    const table = document.querySelector('#spedizioni-table tbody');
    const newRow = document.createElement('tr');
    newRow.setAttribute('data-spedizione-id', spedizione.id);

    newRow.innerHTML = `
        <td class="cliente-nome">${spedizione.cliente.nome}</td>
        <td class="corriere-nome">${spedizione.corriere.nome}</td>
        <td class="mittente">${spedizione.mittente}</td>
        <td class="destinatario">${spedizione.destinatario}</td>
        <td class="stato">${spedizione.stato}</td>
        <td>
            <a href="/Spedizioni/Details/${spedizione.id}">Details</a> |
            <a href="/Spedizioni/Delete/${spedizione.id}">Delete</a>
        </td>
    `;

    table.appendChild(newRow);
    newRow.classList.add('new');
    setTimeout(() => newRow.classList.remove('new'), 2000);
}

// Avvia la connessione
updateConnectionStatus('connecting', 'Connessione in corso...');
connection.start()
    .then(() => {
        updateConnectionStatus('connected', 'Connesso');
    })
    .catch(function (err) {
        updateConnectionStatus('error', 'Errore di connessione');
        console.error(err.toString());
    });