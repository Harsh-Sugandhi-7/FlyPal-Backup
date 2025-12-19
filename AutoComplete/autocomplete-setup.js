//*****************************************
//Created by:  Harsh Sugandhi
//Created on:  4th June 2025
//Created for: FLYPAL-2439 Highlight Not Working Employee.
//*****************************************


/*NOTE: Do not Remove logs added on each steps, they are used for debugging*/

// Function to initialize autocomplete for all elements matching a given selector
function initializeAutocomplete(selector) {

    // Iterate over all elements that match the selector
    $(selector).each(function () {

        var $this = $(this); // Get the current element in the iteration
        var elementId = $this.attr('id') || "NoIDFound"; // Get the actual ID or a placeholder
        console.group("Autocomplete: Attempting to initialize:", elementId, " (Selector:", selector, ")");

        try {

            console.log("  Element found. Checking for data-autocomplete-url...");
            var dataSourceUrl = $this.data('autocomplete-url'); 
            console.log("  Data source URL for", elementId, ":", dataSourceUrl);

            if (!dataSourceUrl) {

                console.error("  Autocomplete: Missing 'data-autocomplete-url' attribute for", elementId);
                console.groupEnd(); // End group for this element
                return; // Skip this element if no URL

            }

            console.log("  Getting textbox width for", elementId, "...");
            // Get the width of the current textbox dynamically
            var textboxWidth = $this.outerWidth();
            console.log("  Textbox width for", elementId, ":", textboxWidth);

             // If you suspect double-initialization issues, you could destroy it first
            // Check if the plugin data exists
            if ($this.data('autocomplete')) {

                console.log("  Autocomplete already initialized on", elementId, ". Destroying previous instance.");
                $this.autocomplete('destroy');

             }

            console.log("  Applying .autocomplete() plugin for", elementId, "...");

            $this.autocomplete(dataSourceUrl, {
                autoFill: true,
                width: textboxWidth,
                mustMatch: true,
                matchContains: true,
                delay: 0,
                success: function (data) {
                    console.log("  SUCCESS callback for", elementId, ": RAW data received:", data);
                    return data;
                },
                parse: function (data) {

                    console.log("  PARSE callback for", elementId, ": Data received:", data);
                    var parsed = [];
                    var lines = data.split('\n');
                    console.log("  PARSE callback for", elementId, ": Number of lines in raw data:", lines.length);

                    for (var i = 0; i < lines.length; i++) {

                        var line = $.trim(lines[i]);
                        if (line) {

                            var parts = line.split('|');
                            if (parts.length === 2) {

                                parsed.push({
                                    data: [parts[0], parts[1]],
                                    value: parts[0], 
                                    result: parts[0]
                                });

                                console.log("  PARSE callback for", elementId, ": Parsed item - Name:", parts[0], "IsWorking:", parts[1]);

                            } else {
                                console.warn("  PARSE callback for", elementId, ": Line did not split into 2 parts or was malformed:", line);
                            }

                        } else {
                            console.log("  PARSE callback for", elementId, ": Empty line encountered after trim.");
                        }
                    }

                    console.log("  PARSE callback for", elementId, ": Final Parsed data array:", parsed);
                    return parsed;

                },
                formatItem: function (row, i, max) {

                    console.log("  FORMAT ITEM callback for", elementId, ":", row[0]);

                    var Name = row[0];
                    var isWorking = row[1];
                    console.log("  FORMAT ITEM callback for", elementId, ": Formatting item - Name:", Name, "IsWorking:", isWorking);

                    if (isWorking === "false") {
                        return "<div class='not-working-item'>" + Name + "</div>";
                    } else {
                        return "<div>" + Name + "</div>";
                    }

                },
                formatResult: function (row) {

                    console.log("  FORMAT RESULT callback for", elementId, ": Formatting result for textbox - Name:", row[0]);
                    return row[0];

                }

            });

            console.log("  .autocomplete() plugin APPLIED successfully for:", elementId);

            console.log("  Attaching keydown event handler for:", elementId, "...");
            $this.on('keydown', function (e) {

                try {

                    console.log("  Keydown event fired on", elementId, ". KeyCode:", e.which || e.keyCode); 
                    const keyCode = e.which || e.keyCode;

                    // Check for Spacebar (keyCode 32) or Ctrl key (keyCode 17)
                    if (keyCode === 32 || keyCode === 17) {

                        e.preventDefault();

                        console.log("  Keydown: Spacebar or Ctrl detected for", elementId, ". Triggering search.");

                        // Trigger the autocomplete search with an empty string.
                        // Your WebMethod needs to return the full list when 'q' is empty.
                        $this.autocomplete().search('');
                        $this.autocomplete().show();

                    }

                } catch (error) {
                    console.error("  Error during Keydown Event for", elementId, ":", error);
                }

            });

            console.log("  Keydown event handler ATTACHED for:", elementId);

            console.log("  Attaching 'result' event handler for:", elementId, "...");
            // Bind to the 'result' event directly after initialization for the current element
            $this.bind('result', function (event, data, formatted) {

                console.group("  'result' event triggered for:", elementId);
                console.log("    Raw 'data' parameter:", data);
                console.log("    Type of 'data':", typeof data);
                console.log("    Is 'data' an Array?:", Array.isArray(data));
                console.log("    Formatted value (what would go into textbox):", formatted);

                if (Array.isArray(data) && data.length >= 2) {

                    var Name = data[0];
                    var isWorking = data[1];

                    console.log("    Result event - Extracted Name:", Name);
                    console.log("    Result event - Extracted IsWorking Status:", isWorking);
                    console.log("    Result event - IsWorking Status type:", typeof isWorking);
                    console.log("    Result event - Comparison 'isWorking === \"false\"' evaluates to:", (isWorking === "false"));

                    if (isWorking === "false") {

                        showMessageBox("Alert!!", Name + " is not working with the Organization.");
                        $this.val(''); // Clear the value of the current textbox

                        console.log("   Action: Textbox cleared due to non-working item.");
                        console.log("Action: Textbox cleared due to non-working item.");
                        console.groupEnd();

                        return false;

                    } else {

                        // Trigger postback for GridView textboxes after successful selection
                        // Only if $this is a gridview-autocomplete textbox
                        if ($this.hasClass('gridview-autocomplete')) {

                            console.log("    Action: Triggering postback for GridView Textbox:", elementId);
                            __doPostBack($this.attr('name'), '');

                        }

                    }

                } else {
                    console.error("Error: 'data' parameter in 'result' event for", elementId, "is not the expected array format.", data);
                }

                console.groupEnd();
                return true;

            });

            console.log("  'result' event handler ATTACHED for:", elementId);
            console.log("Autocomplete Initialization completed successfully for:", elementId);

        } catch (error) {
            console.error("Error during autocomplete Initialization for", elementId, ":", error);
        }

        console.groupEnd(); 

    });

}

function pageLoad(sender, args) {

    initializeAutocomplete(".autocomplete");
    initializeAutocomplete(".gridview-autocomplete");
    console.log("Autocomplete: Initial page load. Initializing autocomplete.");

}

// Call the initialization function when the document is ready
$(document).ready(function () {

    initializeAutocomplete(".autocomplete");
    initializeAutocomplete(".gridview-autocomplete");

});


 //Re-initialize after ASP.NET AJAX partial postbacks, if Sys is defined
if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {

    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {

        // Re-initialize for all textboxes with the class 'autocomplete'
        initializeAutocomplete(".autocomplete");
        initializeAutocomplete(".gridview-autocomplete");

    });

}


// Your custom showMessageBox function (no changes needed here)
function showMessageBox(title, message) {

    console.log("Message Box function started");

    let modalOverlay = document.createElement('div');
    console.log("Div created for Modal" + modalOverlay);

    modalOverlay.className = 'custom-modal-overlay';
    modalOverlay.innerHTML = `
        <div class="custom-modal-content">
            <div class="modal-header">
                <h3>${title}</h3>
            </div>
            <div class="modal-body">
                <div class="modal-message-container">
                    <svg class="modal-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor">
                        <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-6h2v6zm0-8h-2V7h2v2z"/>
                    </svg>
                    <p>${message}</p>
                </div>
            </div>
            <div class="modal-footer">
                <button id="modalCloseButton" class="modal-button">OK</button>
            </div>
        </div>
    `;

    document.body.appendChild(modalOverlay);

    console.log("Modal content" + modalOverlay); 

    $('#modalCloseButton').on('click', () => {

        console.log("Close button clicked"); 
        modalOverlay.remove();

    });

}