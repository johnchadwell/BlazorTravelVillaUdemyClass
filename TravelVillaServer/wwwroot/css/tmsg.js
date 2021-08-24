window.showmsg = (type, message) => {
    if (type === "success") {
        toastr.success(message, "Operation Successful");
    }
    if (type === "error") {
        toastr.error(message, "Operation Failed");
    }
}

window.showsalert = (type, message) => {
    if (type === "success") {
        Swal.fire(
            'Success Msg',
            message,
            'success'
        )
    }
    if (type === "error") {
        Swal.fire(
            'Error Msg',
            message,
            'error'
        )
    }
}

function ShowDeleteConfirmModal() {
    $('#deleteConfirmModal').modal('show');
}

function HideDeleteConfirmModal() {
    $('#deleteConfirmModal').modal('hide');
}
