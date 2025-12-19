
function signout() {
    if (window.event.clientY < 0) {
       
        $.ajax({
            type: "POST",
            url: "TopHeader.aspx/SignOut",
            data: "{}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: "Success"
        });
    }
}



//window.onbeforeunload = signout;



