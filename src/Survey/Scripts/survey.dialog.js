var saveDialog = $("#save-modal");
var workingDialog = $("#working-modal");
var loadingDialog = $("#loading-modal");
var completeDialog = $("#complete-modal");

function WorkingDialogShow() {
    workingDialog.dialog({
        modal: true,
        dialogClass: "no-close",
        draggable: false,
        resizable: false
    });
}

function WorkingDialogClose() {
    workingDialog.dialog("close");
}

function SaveDialogShow() {
    saveDialog.dialog({
        modal: true,
        dialogClass: "no-close",
        draggable: false,
        resizable: false,
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        }
    });
}

function SaveDialogClose() {
    saveDialog.dialog("close");
}

function LoadingDialogShow() {
    loadingDialog.dialog(
        {
            modal: true,
            dialogClass: "no-close",
            draggable: false,
            resizable: false
        });
}

function LoadingDialogClose() {
    loadingDialog.dialog("close");
}

function CompleteDialogShow() {
    completeDialog.dialog(
        {
            modal: true,
            dialogClass: "no-close",
            draggable: false,
            resizable: false
        });
}

function CompleteDialogClose() {
    completeDialog.dialog("close");
}