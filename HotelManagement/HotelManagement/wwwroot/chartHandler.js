function renderCharts(ticketsData, salesData) {
    // Tickets Chart
    const ctxTickets = document.getElementById("ticketsChart").getContext("2d");
    if (window.ticketsChart && typeof window.ticketsChart.destroy === "function") {
        window.ticketsChart.destroy(); // Destroy previous instance if exists
    }
    window.ticketsChart = new Chart(ctxTickets, {
        type: "line",
        data: {
            labels: ["10th Apr", "13th Apr", "16th Apr", "19th Apr", "28th Apr"],
            datasets: [
                {
                    label: "Tickets Sold",
                    data: ticketsData,
                    borderColor: "#007bff",
                    backgroundColor: "rgba(0, 123, 255, 0.1)",
                    fill: true,
                    tension: 0.4,
                },
            ],
        },
        options: {
            responsive: true,
            plugins: { legend: { display: true } },
            scales: { y: { beginAtZero: true } },
        },
    });

    
}
