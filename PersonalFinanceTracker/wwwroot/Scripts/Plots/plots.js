function OnSuccessTransactions(data) {
    console.log({ data });

    var months = [];
    var income = [];
    var expenses = [];

    data.forEach(item => {
        months.push(new Date(item.year, item.month - 1));
        income.push(item.income);
        expenses.push(item.expenses);
    });

    var ctx = document.getElementById('transactionsChart').getContext('2d');
    var transactionsChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: months,
            datasets: [{
                label: 'Income',
                data: income,
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1,
                fill: true
            },
            {
                label: 'Expenses',
                data: expenses,
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 1,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'month',
                        tooltipFormat: 'll',
                    }
                },
                y: {
                    beginAtZero: false,
                    stepSize: 1
                }
            }
        }
    });
}

function OnSuccessResultIncomeCategory(data) {
    var categories = data.category;
    var amount = data.amount;

    const colors = [
        "#5E747F", // Slate Gray
        "#7F9BAE", // Cadet Blue
        "#8FB8DE", // Light Steel Blue
        "#B7C9DB", // Light Blue Gray
        "#A6BBC8", // Blue Bell
        "#8AA2AE", // Cadet
        "#778899", // Light Slate Gray
        "#B0C4DE", // Light Steel Blue
        "#BBC4C2", // Pale Silver
        "#7B8B93", // Cool Gray
        "#6D8273", // Lichen Green
        "#7E9C8D", // Sage Green
        "#89A89B", // Dusty Green
        "#95B2A2", // Sea Green
        "#A5C3B0", // Celadon Green
        "#B6D0C4", // Aquamarine
        "#C2DAD1", // Frosted Mint
        "#CBD8D0", // Green Ash
        "#D4E4DB", // Glacier Green
        "#E0EDE7", // Mist Green
    ];

    var backgroundColors = [];

    for (var i = 0; i < amount.length; i++) {
        var colorIndex = i % colors.length;
        backgroundColors.push(colors[colorIndex]);
    }

    var ctx = document.getElementById('incomeCategoryChart').getContext('2d');
    var categoryChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: categories,
            datasets: [{
                data: amount,
                backgroundColor: backgroundColors,
                borderWidth: 1,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
        }
    });
}

function OnSuccessResultExpensesCategory(data) {
    var categories = data.category;
    var amount = data.amount

    const colors = [
        "#5E747F", // Slate Gray
        "#7F9BAE", // Cadet Blue
        "#8FB8DE", // Light Steel Blue
        "#B7C9DB", // Light Blue Gray
        "#A6BBC8", // Blue Bell
        "#8AA2AE", // Cadet
        "#778899", // Light Slate Gray
        "#B0C4DE", // Light Steel Blue
        "#BBC4C2", // Pale Silver
        "#7B8B93", // Cool Gray
        "#6D8273", // Lichen Green
        "#7E9C8D", // Sage Green
        "#89A89B", // Dusty Green
        "#95B2A2", // Sea Green
        "#A5C3B0", // Celadon Green
        "#B6D0C4", // Aquamarine
        "#C2DAD1", // Frosted Mint
        "#CBD8D0", // Green Ash
        "#D4E4DB", // Glacier Green
        "#E0EDE7", // Mist Green
    ];

    var backgroundColors = [];

    for (var i = 0; i < amount.length; i++) {
        var colorIndex = i % colors.length;
        backgroundColors.push(colors[colorIndex]);
    }

    var ctx = document.getElementById('expensesCategoryChart').getContext('2d');
    var categoryChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: categories,
            datasets: [{
                data: amount,
                borderWidth: 1,
                backgroundColor: backgroundColors,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
        }
    });
}


function OnError(data) {
    console.error('Error fetching data: ', data);
}
