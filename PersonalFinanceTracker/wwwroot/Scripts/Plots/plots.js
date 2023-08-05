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
                backgroundColor: '#7F7FAE',
                borderWidth: 1,
                fill: true
            },
            {
                label: 'Expenses',
                data: expenses,
                backgroundColor: '#CBCCDA',
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
        "#5E5E7F", // Dark Violet Blue
        "#7F7FAE", // Medium Purple
        "#8F8FDE", // Light Pastel Purple
        "#B7B7DB", // Soft Lavender
        "#A6A6C8", // Light Slate Blue
        "#8A8AAE", // Lilac
        "#777789", // Dark Lavender
        "#B0B0DE", // Light Lavender Blue
        "#BBBBC2", // Lilac Grey
        "#7B7B93", // Cool Grey 
        "#6D6D73", // Dark Grayish Olive
        "#7E7E8D", // Dark Grayish Olive Green
        "#89898D", // Grayish Olive Green
        "#9595A2", // Grayish Olive
        "#A5A5B0", // Grayish Lavender
        "#B6B6D0", // Grayish Lilac
        "#C2C2DA", // Light Grayish Lilac
        "#CBCCDA", // Pale Lilac Grey
        "#D4D4E7", // Light Grayish Lavender
        "#E0E0E7", // Light Grayish Blue
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
        "#5E5E7F", // Dark Violet Blue
        "#7F7FAE", // Medium Purple
        "#8F8FDE", // Light Pastel Purple
        "#B7B7DB", // Soft Lavender
        "#A6A6C8", // Light Slate Blue
        "#8A8AAE", // Lilac
        "#777789", // Dark Lavender
        "#B0B0DE", // Light Lavender Blue
        "#BBBBC2", // Lilac Grey
        "#7B7B93", // Cool Grey 
        "#6D6D73", // Dark Grayish Olive
        "#7E7E8D", // Dark Grayish Olive Green
        "#89898D", // Grayish Olive Green
        "#9595A2", // Grayish Olive
        "#A5A5B0", // Grayish Lavender
        "#B6B6D0", // Grayish Lilac
        "#C2C2DA", // Light Grayish Lilac
        "#CBCCDA", // Pale Lilac Grey
        "#D4D4E7", // Light Grayish Lavender
        "#E0E0E7", // Light Grayish Blue
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
