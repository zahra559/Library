_book = {
    ID: 0,
    Name: "",
    Author: "",
    ISBN: "",
    isAvailable: false
}


const SearchBooks = async () => {
    fetch("/Home/GetBooks", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            Name: document.querySelector('#nameSearch').value,
            Author: document.querySelector('#authorSearch').value,
            ISBN: document.querySelector('#isbnSearch').value,
        })
    }).then((response) => {
        return response.ok ? response.json() : OpenToast("Somthing went wrong!");
    }).then(responseJson => {
        if (responseJson.length > 0) {
            $("#booksTable tbody").html("")

            $(responseJson.forEach((book) => {
                $("#booksTable tbody").append(
                    $("<tr>").append(
                        $("<td>").text(book.name),
                        $("<td>").text(book.isbn),
                        $("<td>").text(book.author),
                        ((book.isAvailable) ? $("<button>").addClass("btn btn-success  button-success").text("Borrow").data("book", book)
                            : $("<button>").addClass("btn btn-primary button-primary").text("Return").data("book", book)),
                    )
                )
            }))
        }
        else $("#booksTable tbody").html("")

    })
}

const ReturningBook = async (name, bookID) => {
    fetch("/Home/ReturningBook", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            Book: { ID: bookID },
            User: { Name: name }
        })
    }).then((response) => {
        if (!response.ok)
            OpenToast("You can't return this book either you are not the borrower or you are not a system user!");
        else
            SearchBooks();
    })
}

const BorrowingBook = async (name , bookID) => {
    fetch("/Home/BorrowingBook", {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            Book: { ID: bookID },
            User: { Name: name }
        })
    }).then((response) => {
        if (!response.ok)
        {
            OpenToast("You can't borrow this book now!");
        }
        else
            SearchBooks();
    })
}

document.addEventListener("DOMContentLoaded", function(){

    SearchBooks();

}, false)

function OpenModal() {
    $("#modal").modal("show");
}


function OpenToast(error) {
    const para = document.createElement("p");
    const node = document.createTextNode(error);
    para.appendChild(node);
    const element = document.getElementById("toast-body");
    element.appendChild(para);
    $(".toast").toast("show");
}

$(document).on("click", "#search", function () {
    SearchBooks();
})


$(document).on("click", ".button-success", function () {
    _book = $(this).data("book");
    OpenModal();
})

$(document).on("click", ".button-primary", function () {
    _book = $(this).data("book");
    OpenModal();
})

$(document).on("click", "#submit", function () {
    if ($('#name').val() === "")
        window.alert("User Name is required");

    else {
        if (_book.isAvailable)
            BorrowingBook($('#name').val(),_book.id);
        else ReturningBook($('#name').val(), _book.id); 
        $('#modal').modal('toggle');
    }

})

