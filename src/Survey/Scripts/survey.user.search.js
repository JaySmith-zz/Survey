var selectedUserName = "";

$(function () {

    $("#imageButton").click(ShowSearchDialog);

    $("#linkUserLookup").click(ShowSearchDialog);

    $("#buttonSearch").click(UserSearch);
}
);

function ShowSearchDialog(e) {

    e.preventDefault();

    $("#message").val("");
    $("#selectedUserName").val("");
    $("#search_results tbody tr").remove();
    $("#textboxFirstName").val("");
    $("#textboxLastName").val("");

    var searchForm = $("#find_user");

    searchForm.dialog({
        dialogClass: "no-close",
        modal: true,
        width: 400,
        height: 400,
        draggable: false,
        resizable: false,
        buttons: {
            "OK": function () {
                $("#Owner").val(selectedUserName);
                $(this).dialog("close");
            },
        }
    });


}

function UserSearch(e) {

    var fName = $("#textboxFirstName");
    var lName = $("#textboxLastName");

    if (fName.val() == "" && lName.val() == "") {
        $("#message").val("Please enter First Name and/or Last Name");
    } else {

        e.preventDefault();

        $("#search_results tbody tr").remove();

        var search = {};
        search.firstName = fName.val();
        search.lastName = lName.val();

        $.ajax({
            url: '/UserSearch/Search',
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify(search),
            async: false,
            contentType: 'application/json: charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(xhr.responseText);
                alert(thrownError);
            },
            success: function (data) {
                var table = $("#search_results tbody");

                $.each(data, function (id, user) {

                    table.append("<tr>" +
                        "<td>" + user.FirstName + " " + user.LastName + "</td>" +
                        "<td id='userName'>" + user.NtUserAccount + "</tr>" +
                        "</tr>");
                });

                $('#search_results tbody tr').click(function () {
                    $("#search_results tr td").css("background-color", "");
                    $(this).children("tbody td, tbody th").css("background-color", "orange");
                    selectedUserName = $(this).find("#userName").text();
                });
            },
            async: false
        });

        $("#search_results").css("border-collapse", "collapse");
        $("#search_results tbody").find("tr:odd").css("background-color", "silver");
    }

}