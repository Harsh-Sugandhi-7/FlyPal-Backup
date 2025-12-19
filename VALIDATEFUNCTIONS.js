//-------------------------------------------------------------------
// Trim functions
//   Returns string with whitespace trimmed
//-------------------------------------------------------------------

function LTrim(str) {
    if (str == null) { return null; }
    for (var i = 0; str.charAt(i) == " "; i++);
    return str.substring(i, str.length);
}
function RTrim(str) {
    if (str == null) { return null; }
    for (var i = str.length - 1; str.charAt(i) == " "; i--);
    return str.substring(0, i + 1);
}
function Trim(str) { return LTrim(RTrim(str)); }
function LTrimAll(str) {
    if (str == null) { return str; }
    for (var i = 0; str.charAt(i) == " " || str.charAt(i) == "\n" || str.charAt(i) == "\t"; i++);
    return str.substring(i, str.length);
}
function RTrimAll(str) {
    if (str == null) { return str; }
    for (var i = str.length - 1; str.charAt(i) == " " || str.charAt(i) == "\n" || str.charAt(i) == "\t"; i--);
    return str.substring(0, i + 1);
}
function TrimAll(str) {
    return LTrimAll(RTrimAll(str));
}
//-------------------------------------------------------------------
// isNull(value)
//   Returns true if value is null
//-------------------------------------------------------------------
function isNull(val) { return (val == null); }

//-------------------------------------------------------------------
// isBlank(value)
//   Returns true if value only contains spaces
//-------------------------------------------------------------------
function isBlank(val) {
    if (val == null) { return true; }
    for (var i = 0; i < val.length; i++) {
        if ((val.charAt(i) != ' ') && (val.charAt(i) != "\t") && (val.charAt(i) != "\n") && (val.charAt(i) != "\r")) { return false; }
    }
    return true;
}

//-------------------------------------------------------------------
// isInteger(value)
//   Returns true if value contains all digits
//-------------------------------------------------------------------
function isInteger(val) {
    if (isBlank(val)) { return false; }
    for (var i = 0; i < val.length; i++) {
        if (!isDigit(val.charAt(i))) { return false; }
    }
    return true;
}

//-------------------------------------------------------------------
// isNumeric(value)
//   Returns true if value contains a positive float value
//-------------------------------------------------------------------
function isNumeric(val) { return (parseFloat(val, 10) == (val * 1)); }

//-------------------------------------------------------------------
// isArray(obj)
// Returns true if the object is an array, else false
//-------------------------------------------------------------------
function isArray(obj) { return (typeof (obj.length) == "undefined") ? false : true; }

//-------------------------------------------------------------------
// isDigit(value)
//   Returns true if value is a 1-character digit
//-------------------------------------------------------------------
//--------------------------------------------------------------USERNAME
function validCharacters(type, len, e) {

    if (type == "validCharacter") {

        if ((e.charCode < 65 || e.charCode > 90) && (e.charCode < 97 || e.charCode > 122) && (e.charCode != 46) && (e.charCode != 32) && (e.charCode != 47)) {
            alert("Enter Valid Characters Only...");
            e.preventDefault();
            event.returnValue = false;
        }
    }
}
//--------------------------------------------------------------



function validateText(type, len, e) {
    // For Alphabets

    if (type == "A") {

        if (len.length >= 95) {
            e.preventDefault();
            event.returnValue = false;
        }
        // If the keycode of the pressed key is not in the required range then return false.
        if ((e.charCode < 65 || e.charCode > 90) && (e.charCode < 97 || e.charCode > 122) && (e.charCode != 46) && (e.charCode != 32) && (e.charCode != 47)) {
            e.preventDefault();
            event.returnValue = false;
        }

    }
    // Only Alphabets along with . and /
    else if (type == "EMAIL") {
        //alert(event.keyCode);
        if ((e.charCode < 47 || e.charCode > 57) && (e.charCode < 64 || e.charCode > 90) && (e.charCode < 97 || e.charCode > 122) && (e.charCode != 46) && (e.charCode != 32) && (e.charCode != 47)) {
            if (e.charCode == 45 || e.charCode == 95) {
                e.preventDefault();
                event.returnValue = false;
            }
            else {
                alert("Enter Valid Email Characters...");
                e.preventDefault();
                event.returnValue = false;
            }
        }

        if (e.charCode == 47) {
            alert("Enter Valid Email Characters...");
            e.preventDefault();
            event.returnValue = false;
        }
    }


    //for username
    else if (type == "USERNAME") {

        if (len.length >= 40) {
            e.preventDefault();
            event.returnValue = false;
        }
        // If the keycode of the pressed key is not in the required range then return false.
        if ((e.charCode < 65 || e.charCode > 90) && (e.charCode < 97 || e.charCode > 122) && (e.charCode != 46) && (e.charCode != 32) && (e.charCode != 47)) {
            alert("Enter Valid Characters Only...");
            e.preventDefault();
            event.returnValue = false;
        }

    }
    //for username
    else if (type == "PASSWORD") {

        if (len.length >= 40) {
            e.preventDefault();
            event.returnValue = false;
        }
    }
    // Only numbers
    else if (type == "N") {
        // If the keycode of the pressed key is not in the required range then return false.
        if ((e.charCode < 48 || e.charCode > 57) && e.code != 'Backspace' && e.code != 'Delete' && e.code != 'ArrowLeft' && e.code != 'ArrowRight') {
            alert("Enter Numbers Only...");
            e.preventDefault();
            event.returnValue = false;


        }
    }

    else if (type == "ValidDate") {
        // If the keycode of the pressed key is not in the required range then return false.
        if ((e.charCode < 48 || e.charCode > 57) && (e.charCode < 65 || e.charCode > 90) && (e.charCode < 97 || e.charCode > 122) && (e.charCode != 46) && (e.charCode != 13) && (e.charCode != 32) && (e.charCode != 47) && (e.charCode != 45) && (e.charCode != 95) && (e.charCode != 64) && e.code != 'Backspace' && e.code != 'Delete' && e.code != 'ArrowLeft' && e.code != 'ArrowRight') {
            alert("Enter Valid first Characters...");
            e.preventDefault();
            event.returnValue = false;

        }
    }
    // Only numbers
    else if (type == "D") {
        // If the keycode of the pressed key is not in the required range then return false.
        if ((e.charCode < 48 || e.charCode > 57) && (e.charCode != 46) && e.code != 'Backspace' && e.code != 'Delete' && e.code != 'ArrowLeft' && e.code != 'ArrowRight') {
            alert("Enter Numbers Only...");
            e.preventDefault();
            event.returnValue = false;

        }
    }
    //Added By Shweta on 1-Aug-2012////////
    else if (type == "ND") //Allows positive and negative numbers
    {

        if ((e.charCode < 48 || e.charCode > 57) && (e.charCode != 46) && (e.charCode != 45) && e.code != 'Backspace' && e.code != 'Delete' && e.code != 'ArrowLeft' && e.code != 'ArrowRight') {
            alert("Enter Numbers Only...");
            e.preventDefault();
            event.returnValue = false;

        }
    }
    ////////
    //Added By Prashant Potpite 09-Apr-2010//////////////////////////////////////////////////////////////////////////////////////
    else if (type = "NUM") {
        //if (event.keyCode != 45 || event.keyCode != 13 || event.keyCode != 8 || event.keyCode != 9 || event.keyCode != 189 || (event.keyCode <= 48 && event.keyCode >= 58)) {
        if ((e.charCode < 48 || e.charCode > 58) && e.code != 'Backspace' && e.code != 'Delete' && e.code != 'ArrowLeft' && e.code != 'ArrowRight') {// && (event.keyCode != 58)) {
            alert("Enter Numbers Only...");
            e.preventDefault();
            event.returnValue = false;
        }

    }
    ////////////////////////////////////////////////////////////////////////////////////////

    //Added By Prashant Potpite 30-Jun-2011//////////////////////////////////////////////////////////////////////////////////////
    //else if (type = "USERNAME1") {
    // if ((event.keyCode < 65 || event.keyCode > 90) && (event.keyCode < 97 || event.keyCode > 122) && (event.keyCode != 46) && (event.keyCode != 32) && (event.keyCode != 47))
    // alert("Enter Valid Characters Only...");
    // event.returnValue = false;
    //}

    //}
    ////////////////////////////////////////////////////////////////////////////////////////

    else if (type == "PINCODE") {
        // If the keycode of the pressed key is not in the required range then return false.
        if ((e.charCode < 48 || e.charCode > 57) && (e.charCode != 32)) {
            alert("Enter Numbers Only...");
            e.preventDefault();
            event.returnValue = false;

        }
    }

    // It allows numbers and -
    else if (type == "PHNO") {
        // If the keycode of the pressed key is not in the required range then return false.
        if ((e.charCode < 48 || e.charCode > 57) && (e.charCode != 45) && (e.charCode != 40) && (e.charCode != 41) && (e.charCode != 43) && (e.charCode != 39) && (e.charCode != 32) && (e.charCode != 47)) {
            alert("Enter Numbers Only...");
            e.preventDefault();
            event.returnValue = false;
        }
    }

    // It allows numbers and /
    else if (type == "DATE") {

        // If the keycode of the pressed key is not in the required range then return false.
        if ((e.charCode < 48 || e.charCode > 57) && (e.charCode != 47)) {
            e.preventDefault();
            event.returnValue = false;
        }
        if (len.length > 9) {
            e.preventDefault();
            event.returnValue = false;
        }
    }

}


function validateDecimalNo(Control, e) {
    if ((e.charCode < 48 || e.charCode > 57) && (e.charCode != 46) && e.code != 'Backspace' && e.code != 'Delete' && e.code != 'ArrowLeft' && e.code != 'ArrowRight') {
        alert("Enter Numbers Only...");
        e.preventDefault();
        event.returnValue = false;
    }
    else if (e.charCode == 46 && e.code != 'Backspace' && e.code != 'Delete' && e.code != 'ArrowLeft' && e.code != 'ArrowRight') {
        var str = Control.value;
        var n = str.match(new RegExp('\\.', 'g'));
        if (n != null) {
            if (n.length >= 1) {
                alert("Enter Valid Number...");
                e.preventDefault();
                event.returnValue = false;
            }
        }
    }
}

function validateIntegerNo(Control, e) {
    if ((e.charCode < 48 || e.charCode > 58) && e.code != 'Backspace' && e.code != 'Delete' && e.code != 'ArrowLeft' && e.code != 'ArrowRight') {// && (event.keyCode != 58)) {
        alert("Enter Numbers Only...");
        e.preventDefault();
        event.returnValue = false;
    }
    else if (e.charCode == 46 && e.code != 'Backspace' && e.code != 'Delete' && e.code != 'ArrowLeft' && e.code != 'ArrowRight') {
        var str = Control.value;
        var n = str.match(new RegExp('\\.', 'g'));
        if (n != null) {
            if (n.length >= 1) {
                alert("Enter Valid Number...");
                e.preventDefault();
                event.returnValue = false;
            }
        }
    }
}







function isDigit(num) {
    if (num.length > 1) { return false; }
    var string = "1234567890";
    if (string.indexOf(num) != -1) { return true; }
    return false;
}

//-------------------------------------------------------------------
// setNullIfBlank(input_object)
//   Sets a form field to "" if it isBlank()
//-------------------------------------------------------------------
function setNullIfBlank(obj) { if (isBlank(obj.value)) { obj.value = ""; } }

//-------------------------------------------------------------------
// setFieldsToUpperCase(input_object)
//   Sets value of form field toUpperCase() for all fields passed
//-------------------------------------------------------------------
function setFieldsToUpperCase() {
    for (var i = 0; i < arguments.length; i++) {
        arguments[i].value = arguments[i].value.toUpperCase();
    }
}

//-------------------------------------------------------------------
// disallowBlank(input_object[,message[,true]])
//   Checks a form field for a blank value. Optionally alerts if 
//   blank and focuses
//-------------------------------------------------------------------
function disallowBlank(obj) {
    var msg = (arguments.length > 1) ? arguments[1] : "";
    var dofocus = (arguments.length > 2) ? arguments[2] : false;
    if (isBlank(getInputValue(obj))) {
        if (!isBlank(msg)) { alert(msg); }
        if (dofocus) {
            if (isArray(obj) && (typeof (obj.type) == "undefined")) { obj = obj[0]; }
            if (obj.type == "text" || obj.type == "textarea" || obj.type == "password") { obj.select(); }
            obj.focus();
        }
        return true;
    }
    return false;
}

//-------------------------------------------------------------------
// disallowModify(input_object[,message[,true]])
//   Checks a form field for a value different than defaultValue. 
//   Optionally alerts and focuses
//-------------------------------------------------------------------
function disallowModify(obj) {
    var msg = (arguments.length > 1) ? arguments[1] : "";
    var dofocus = (arguments.length > 2) ? arguments[2] : false;
    if (getInputValue(obj) != getInputDefaultValue(obj)) {
        if (!isBlank(msg)) { alert(msg); }
        if (dofocus) {
            if (isArray(obj) && (typeof (obj.type) == "undefined")) { obj = obj[0]; }
            if (obj.type == "text" || obj.type == "textarea" || obj.type == "password") { obj.select(); }
            obj.focus();
        }
        setInputValue(obj, getInputDefaultValue(obj));
        return true;
    }
    return false;
}

//-------------------------------------------------------------------
// commifyArray(array[,delimiter])
//   Take an array of values and turn it into a comma-separated string
//   Pass an optional second argument to specify a delimiter other than
//   comma.
//-------------------------------------------------------------------
function commifyArray(obj, delimiter) {
    if (typeof (delimiter) == "undefined" || delimiter == null) {
        delimiter = ",";
    }
    var s = "";
    if (obj == null || obj.length <= 0) { return s; }
    for (var i = 0; i < obj.length; i++) {
        s = s + ((s == "") ? "" : delimiter) + obj[i].toString();
    }
    return s;
}

//-------------------------------------------------------------------
// getSingleInputValue(input_object,use_default,delimiter)
//   Utility function used by others
//-------------------------------------------------------------------
function getSingleInputValue(obj, use_default, delimiter) {
    switch (obj.type) {
        case 'radio': case 'checkbox': return (((use_default) ? obj.defaultChecked : obj.checked) ? obj.value : null);
        case 'text': case 'hidden': case 'textarea': return (use_default) ? obj.defaultValue : obj.value;
        case 'password': return ((use_default) ? null : obj.value);
        case 'select-one':
            if (obj.options == null) { return null; }
            if (use_default) {
                var o = obj.options;
                for (var i = 0; i < o.length; i++) { if (o[i].defaultSelected) { return o[i].value; } }
                return o[0].value;
            }
            if (obj.selectedIndex < 0) { return null; }
            return (obj.options.length > 0) ? obj.options[obj.selectedIndex].value : null;
        case 'select-multiple':
            if (obj.options == null) { return null; }
            var values = new Array();
            for (var i = 0; i < obj.options.length; i++) {
                if ((use_default && obj.options[i].defaultSelected) || (!use_default && obj.options[i].selected)) {
                    values[values.length] = obj.options[i].value;
                }
            }
            return (values.length == 0) ? null : commifyArray(values, delimiter);
    }
    alert("FATAL ERROR: Field type " + obj.type + " is not supported for this function");
    return null;
}

//-------------------------------------------------------------------
// getSingleInputText(input_object,use_default,delimiter)
//   Utility function used by others
//-------------------------------------------------------------------
function getSingleInputText(obj, use_default, delimiter) {
    switch (obj.type) {
        case 'radio': case 'checkbox': return "";
        case 'text': case 'hidden': case 'textarea': return (use_default) ? obj.defaultValue : obj.value;
        case 'password': return ((use_default) ? null : obj.value);
        case 'select-one':
            if (obj.options == null) { return null; }
            if (use_default) {
                var o = obj.options;
                for (var i = 0; i < o.length; i++) { if (o[i].defaultSelected) { return o[i].text; } }
                return o[0].text;
            }
            if (obj.selectedIndex < 0) { return null; }
            return (obj.options.length > 0) ? obj.options[obj.selectedIndex].text : null;
        case 'select-multiple':
            if (obj.options == null) { return null; }
            var values = new Array();
            for (var i = 0; i < obj.options.length; i++) {
                if ((use_default && obj.options[i].defaultSelected) || (!use_default && obj.options[i].selected)) {
                    values[values.length] = obj.options[i].text;
                }
            }
            return (values.length == 0) ? null : commifyArray(values, delimiter);
    }
    alert("FATAL ERROR: Field type " + obj.type + " is not supported for this function");
    return null;
}

//-------------------------------------------------------------------
// setSingleInputValue(input_object,value)
//   Utility function used by others
//-------------------------------------------------------------------
function setSingleInputValue(obj, value) {
    switch (obj.type) {
        case 'radio': case 'checkbox': if (obj.value == value) { obj.checked = true; return true; } else { obj.checked = false; return false; }
        case 'text': case 'hidden': case 'textarea': case 'password': obj.value = value; return true;
        case 'select-one': case 'select-multiple':
            var o = obj.options;
            for (var i = 0; i < o.length; i++) {
                if (o[i].value == value) { o[i].selected = true; }
                else { o[i].selected = false; }
            }
            return true;
    }
    alert("FATAL ERROR: Field type " + obj.type + " is not supported for this function");
    return false;
}

//-------------------------------------------------------------------
// getInputValue(input_object[,delimiter])
//   Get the value of any form input field
//   Multiple-select fields are returned as comma-separated values, or
//   delmited by the optional second argument
//   (Doesn't support input types: button,file,reset,submit)
//-------------------------------------------------------------------
function getInputValue(obj, delimiter) {
    var use_default = (arguments.length > 2) ? arguments[2] : false;
    if (isArray(obj) && (typeof (obj.type) == "undefined")) {
        var values = new Array();
        for (var i = 0; i < obj.length; i++) {
            var v = getSingleInputValue(obj[i], use_default, delimiter);
            if (v != null) { values[values.length] = v; }
        }
        return commifyArray(values, delimiter);
    }
    return getSingleInputValue(obj, use_default, delimiter);
}

//-------------------------------------------------------------------
// getInputText(input_object[,delimiter])
//   Get the displayed text of any form input field
//   Multiple-select fields are returned as comma-separated values, or
//   delmited by the optional second argument
//   (Doesn't support input types: button,file,reset,submit)
//-------------------------------------------------------------------
function getInputText(obj, delimiter) {
    var use_default = (arguments.length > 2) ? arguments[2] : false;
    if (isArray(obj) && (typeof (obj.type) == "undefined")) {
        var values = new Array();
        for (var i = 0; i < obj.length; i++) {
            var v = getSingleInputText(obj[i], use_default, delimiter);
            if (v != null) { values[values.length] = v; }
        }
        return commifyArray(values, delimiter);
    }
    return getSingleInputText(obj, use_default, delimiter);
}

//-------------------------------------------------------------------
// getInputDefaultValue(input_object[,delimiter])
//   Get the default value of any form input field when it was created
//   Multiple-select fields are returned as comma-separated values, or
//   delmited by the optional second argument
//   (Doesn't support input types: button,file,password,reset,submit)
//-------------------------------------------------------------------
function getInputDefaultValue(obj, delimiter) { return getInputValue(obj, delimiter, true); }

//-------------------------------------------------------------------
// isChanged(input_object)
//   Returns true if input object's value has changed since it was
//   created.
//-------------------------------------------------------------------
function isChanged(obj) { return (getInputValue(obj) != getInputDefaultValue(obj)); }

//-------------------------------------------------------------------
// setInputValue(obj,value)
//   Set the value of any form field. In cases where no matching value
//   is available (select, radio, etc) then no option will be selected
//   (Doesn't support input types: button,file,password,reset,submit)
//-------------------------------------------------------------------
function setInputValue(obj, value) {
    var use_default = (arguments.length > 1) ? arguments[1] : false;
    if (isArray(obj) && (typeof (obj.type) == "undefined")) {
        for (var i = 0; i < obj.length; i++) { setSingleInputValue(obj[i], value); }
    }
    else { setSingleInputValue(obj, value); }
}

//-------------------------------------------------------------------
// isFormModified(form_object,hidden_fields,ignore_fields)
//   Check to see if anything in a form has been changed. By default
//   it will check all visible form elements and ignore all hidden 
//   fields. 
//   You can pass a comma-separated list of field names to check in
//   addition to visible fields (for hiddens, etc).
//   You can also pass a comma-separated list of field names to be
//   ignored in the check.
//-------------------------------------------------------------------
function isFormModified(theform, hidden_fields, ignore_fields) {
    if (hidden_fields == null) { hidden_fields = ""; }
    if (ignore_fields == null) { ignore_fields = ""; }
    var hiddenFields = new Object();
    var ignoreFields = new Object();
    var i, field;
    var hidden_fields_array = hidden_fields.split(',');
    for (i = 0; i < hidden_fields_array.length; i++) {
        hiddenFields[Trim(hidden_fields_array[i])] = true;
    }
    var ignore_fields_array = ignore_fields.split(',');
    for (i = 0; i < ignore_fields_array.length; i++) {
        ignoreFields[Trim(ignore_fields_array[i])] = true;
    }
    for (i = 0; i < theform.elements.length; i++) {
        var changed = false;
        var name = theform.elements[i].name;
        if (!isBlank(name)) {
            var type = theform[name].type;
            if (!ignoreFields[name]) {
                if (type == "hidden" && hiddenFields[name]) { changed = isChanged(theform[name]); }
                else if (type == "hidden") { changed = false; }
                else { changed = isChanged(theform[name]); }
            }
        }
        if (changed) { return true; }
    }
    return false;
}

