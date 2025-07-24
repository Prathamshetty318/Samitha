
let selectedYear = new Date().getFullYear();

function updateSummary(records) {
    let income = 0, expense = 0;
    records.forEach(r => {
        if (r.type.toLowerCase() === "donation") income += r.amount;
        else expense += r.amount;
    });

    $("#totalIncome").text(`₹${income}`);
    $("#totalExpense").text(`₹${expense}`);
    $("#netBalance").text(`₹${income - expense}`);
}



function loadRecords(year) {
    selectedYear = year;
    $.get(`/api/festival/get-by-year/${year}`, function (records) {
        const tbody = $("#recordsTable tbody").empty();

        records.forEach(r => {
            let row = `<tr>
                <td class="truncate clickable-text" title="${r.description}" data-bs-toggle="modal" data-bs-target="#descModal" data-desc="${r.description}">${r.description}</td>
                <td>${r.amount}</td>
                <td>${r.type}</td>
                <td>${r.year}</td>`;

            // ✅ Only add Delete button if hasAccess is 1
            if (hasAccess === 1) {
                row += `<td><button class="btn btn-sm btn-danger" onclick="deleteRecord(${r.id})">Delete</button></td>`;
            }

            row += `</tr>`;
            tbody.append(row);
        });

        // 💡 Attach filtering after DOM update
        $("#searchInput").off("keyup").on("keyup", function () {
            const value = $(this).val().toLowerCase();
            $("#recordsTable tbody tr").filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
            });
        });

        updateSummary(records);
    });
}


function deleteRecord(id) {

    if (!id || id === 0) {
        return;
    }

    $.ajax({
        url: `/api/festival/delete/${id}`,
        type: "DELETE"
    }).done(() => loadRecords(selectedYear));
}

$("#recordForm").submit(function (e) {
    e.preventDefault();

    const form = this;

    const data = {
        description: form.desc.value,
        amount: parseFloat(form.amt.value),
        type: form.type.value,
        year: selectedYear
    };

    $.ajax({
        url: "/api/festival/add",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(data)
    }).done(() => {
        form.reset();
        loadRecords(selectedYear);
        populateYearDropdown();
    });
});

function populateYearDropdown() {
    const current = new Date().getFullYear();
    const select = $("#yearFilter");
    select.empty();
    for (let y = current; y >= 2020; y--) {
        select.append(`<option value="${y}">${y}</option>`);
    }
    select.val(selectedYear);
}

$(document).ready(() => {
    populateYearDropdown();
    $("#yearFilter").change(function () {
        loadRecords(this.value);
    });
    loadRecords(selectedYear);
});

let pieChart;

function updateChart(income, expense) {
    const chartData = {
        labels: ['Income', 'Expense'],
        datasets: [{
            data: [income, expense],
            backgroundColor: ['#28a745', '#dc3545']
        }]
    };

    const options = {
        responsive: true,
        plugins: {
            legend: {
                position: 'bottom'
            }
        }
    };

    // Desktop Chart
    const ctxDesktop = document.getElementById('pieChart');
    if (ctxDesktop) {
        if (window.pieChartDesktop instanceof Chart) {
            window.pieChartDesktop.data = chartData;
            window.pieChartDesktop.update();
        } else {
            window.pieChartDesktop = new Chart(ctxDesktop, {
                type: 'pie',
                data: chartData,
                options: options
            });
        }
    }

    // Mobile Chart
    const ctxMobile = document.getElementById('pieChartMobile');
    if (ctxMobile) {
        if (window.pieChartMobile instanceof Chart) {   
            window.pieChartMobile.data = chartData;
            window.pieChartMobile.update();
        } else {
            window.pieChartMobile = new Chart(ctxMobile, {
                type: 'pie',
                data: chartData,
                options: options
            });
        }
    }
}


function updateSummary(records) {
    let income = 0, expense = 0;
    records.forEach(r => {
        if (r.type.toLowerCase() === "donation") income += r.amount;
        else expense += r.amount;
    });

    $("#totalIncome").text(`₹${income}`);
    $("#totalExpense").text(`₹${expense}`);
    $("#netBalance").text(`₹${income - expense}`);

    updateChart(income, expense);
}

