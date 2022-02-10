var Detail = function (Id) {
    var url = "/Transaction/Detail/" + Id;
    $('#detail-modal-title').html("Details for Transaction #" + Id);
    $("#DetailTransactionModalBody").load(url, function () {
        $("#DetailTransactionModal").modal("show");
    });
}

var UpSert = function (Id, isIncome) {
    var url = "/Transaction/UpSert?Id=" + Id + "&isIncome=" + isIncome;
    if (Id == 0 && isIncome == true)
        $('#upsert-modal-title').html("Create Income");
    else if (Id == 0 && isIncome == false)
        $('#upsert-modal-title').html("Create Expense");
    else if (Id > 0 && isIncome == true)
        $('#upsert-modal-title').html("Edit Income #" + Id);
    else if (Id > 0 && isIncome == false)
        $('#upsert-modal-title').html("Edit Expense #" + Id);

    $("#UpSertTransactionModalBody").load(url, function () {
        $("#UpSertTransactionModal").modal("show");
    });
}

var Delete = function (Id) {
    var url = "/Transaction/Delete/" + Id;
    $('#delete-modal-title').html("Delete Transaction #" + Id);
    $("#DeleteTransactionModalBody").load(url, function () {
        $("#DeleteTransactionModal").modal("show");
    });
}

window.recordAction = function (args) {
    var element;
    var isIncome = args['Category'].CategoryType.Name.localeCompare("Income") === 0 ? true : false;
    element = '<div class="w-50" role="group">'
        + '<a style="font-size: 14px !important;" onclick="Detail(' + args['Id'] + ')" class="btn btn-secondary btn-sm mx-2" data-bs-toggle="tooltip" data-bs-placement="top" title="View Details">'
        + '<i class="fas fa-info-circle"></i>'
        + '</a>'
        + '<a style="font-size: 14px !important;" onclick="UpSert(' + args['Id'] + ',' + isIncome + ') ' + '" class="btn btn-primary btn-sm mx-2" data-bs-toggle="tooltip" data-bs-placement="top" title="Edit">'
        + '<i class="fas fa-edit"></i>'
        + '</a>'
        + '<a style="font-size: 14px !important;" onclick="Delete(' + args['Id'] + ')" class="btn btn-danger btn-sm mx-2" data-bs-toggle="tooltip" data-bs-placement="top" title="Delete">'
        + '<i class="far fa-trash-alt"></i>'
        + '</a>'
        + '</div>';
    return element;
}

function rowDataBound(args) {
    if (this.pagerModule.pagerObj.currentPage == 1) {
        args.row.querySelector('td').innerText = +args.row.getAttribute('aria-rowindex') + 1;
    }
    else {
        var num = (this.pagerModule.pagerObj.currentPage - 1) * this.pagerModule.pageSettings.pageSize;
        args.row.querySelector('td').innerText = +args.row.getAttribute('aria-rowindex') + 1 + num;
    }
}


