<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StickyNote.aspx.vb" Inherits="Flypal.StickyNote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <meta http-equiv="x-ua-compatible" content="IE=7,8,9" />
    <title></title>
    <link href="StickNote.css" id="MainStyle" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="jquery-1.6.1.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.js" integrity="sha256-H+K7U5CnXl1h5ywQfKtSj8PCmoN9aaq30gDh27Xc0jk="
        crossorigin="anonymous"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            all_notes = $("li a"); all_notes.on("keyup", function 
    () {
                note_title = $(this).find("h2").text(); note_content = $(this).find("p").text();
                item_key = "list_" + $(this).parent().index(); data = { title: note_title, content:
    note_content
                }; window.localStorage.setItem(item_key, JSON.stringify(data));
            });
            all_notes.each(function (index) {
                data = JSON.parse(window.localStorage.getItem("list_"
    + index)); if (data !== null) {
                    note_title = data.title; note_content = data.content;
                    $(this).find("h2").text(note_title); $(this).find("p").text(note_content);
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ul>
            <li><a href="#" contenteditable>
                <h2>
                    Title #1</h2>
                <p>
                    Text Content #1</p>
            </a></li>
            <li><a href="#" contenteditable>
                <h2>
                    Title #2</h2>
                <p>
                    Text Content #2</p>
            </a></li>
            <li><a href="#" contenteditable>
                <h2>
                    Title #3</h2>
                <p>
                    Text Content #3</p>
            </a></li>
            <li><a href="#" contenteditable>
                <h2>
                    Title #4</h2>
                <p>
                    Text Content #4</p>
            </a></li>
            <li><a href="#" contenteditable>
                <h2>
                    Title #5</h2>
                <p>
                    Text Content #5</p>
            </a></li>
            <li><a href="#" contenteditable>
                <h2>
                    Title #6</h2>
                <p>
                    Text Content #6</p>
            </a></li>
            <li><a href="#" contenteditable>
                <h2>
                    Title #7</h2>
                <p>
                    Text Content #7</p>
            </a></li>
            <li><a href="#" contenteditable>
                <h2>
                    Title #8</h2>
                <p>
                    Text Content #8</p>
            </a></li>
        </ul>
    </div>
    </form>
</body>
</html>
