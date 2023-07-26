function OnSuccessTransactionsPreview(data) {
    var tableBody = $("#transactionsTable");

    data.forEach(function (transaction) {
        var row = "<tr>" +
            "<td>" + transaction.category + "</td>" +
            "<td>" + new Date(transaction.date).toLocaleDateString() + "</td>" +
            "<td>" + transaction.description + "</td>" +
            "<td>" + transaction.amount + "</td>" +
            "</tr>";
        tableBody.append(row);
    });
}

function OnSuccessBudgetPlansPreview(data) {
    var tableBody = $("#budgetPlansTable");

    data.forEach(function (budget) {
        var row = "<tr>" +
            "<td>" + budget.targetAmount + "</td>" +
            "<td>" + budget.currentAmount + "</td>" +
            "<td>" + new Date(budget.deadline).toLocaleDateString() + "</td>" +
            "<td>" + budget.status + "</td>" +
            "</tr>";
        tableBody.append(row);
    });
}
