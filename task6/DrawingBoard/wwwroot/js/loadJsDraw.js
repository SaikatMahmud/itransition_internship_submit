var editor;
var connectionLoadCanvas;
var boardId;
var username;

$(document).ready(function () {
    var pathname = window.location.pathname;
    boardId = pathname.split('/').pop();

    Swal.fire({
        title: "Whats you name?",
        input: "text",
        inputLabel: "Enter your name to start drawing",
        inputPlaceholder: "Enter your nickname",
        showCancelButton: true,
        inputValidator: (inputValue) => {
            if (!inputValue) {
                return "You need to provide your name!";
            }
        }
    }).then((result) => {
        if (result.value) {
            username = result.value;
            startDrawing();
            startCommentConnection(boardId);
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            window.location.href = "/";
        }
    });
});

function startDrawing() {
    const settings = {
        wheelEventsEnabled: 'only-if-focused',
    };
    editor = new jsdraw.Editor(document.body, settings);
    const toolbar = editor.addToolbar();

    toolbar.addExitButton(() => {
        editor.remove();
        window.location.href = "/";
    });

    toolbar.addActionButton('|Sync|', () => {
        editor.remove();
        startDrawing();
    });

    toolbar.addActionButton('|Download|', () => {
        var jpgDataUrl = editor.toDataURL();
        console.log(jpgDataUrl);
        download(jpgDataUrl, `drawing-${boardId}.jpg`);
    });


    editor.getRootElement().style.height = '95vh';
    editor.getRootElement().style.border = '2px solid gray';

    const addToHistory = false;
    editor.dispatch(editor.setBackgroundStyle({
        //type: jsdraw.BackgroundComponentBackgroundType.Grid,
        autoresize: true,
    }), addToHistory);

    getExistingSvg();

    editor.notifier.on(jsdraw.EditorEventType.CommandDone, (evt) => {
        if (evt.kind !== jsdraw.EditorEventType.CommandDone) {
            throw new Error('Incorrect event type');
        }

        if (evt.command instanceof jsdraw.SerializableCommand) {
            postToServer(JSON.stringify({
                //clientId,
                command: evt.command.serialize()
            }));
            saveNewSvg();
        } else {
            console.log('!', evt.command, 'instanceof jsdraw.SerializableCommand');
        }
    });

    editor.notifier.on(jsdraw.EditorEventType.CommandUndone, (evt) => {
        if (evt.kind !== jsdraw.EditorEventType.CommandUndone) {
            return;
        }

        if (!(evt.command instanceof jsdraw.SerializableCommand)) {
            console.log('Not serializable!', evt.command);
            return;
        }

        postToServer(JSON.stringify({
            //clientId,
            command: jsdraw.invertCommand(evt.command).serialize()
        }));
        saveNewSvg();
    });
}

function startCommentConnection(boardId) {
    connectionLoadCanvas = new signalR.HubConnectionBuilder().withUrl("/hub/boardhub", signalR.HttpTransportType.WebSockets).build();

    connectionLoadCanvas.on("UpdateDrawing", (drawingData) => {
        console.log("Received new command");
        processUpdates(drawingData);
    });

    connectionLoadCanvas.on("ReceiveUserJoinInfo", (username) => {
        console.log("Received new user joined: " + username);
        toastr.info(username + " joined the board");

    });
    connectionLoadCanvas.start().then(() => {
        connectionLoadCanvas.invoke("JoinBoard", boardId.toString(), username).then((msg) =>
            console.log(msg))
    }).catch(err => console.error(err.toString()));
}

function getExistingSvg() {
    $.ajax({
        type: 'GET',
        url: `/get-svg/${boardId}`,
        dataType: 'text',
        success: function (svgText) {
            if (svgText.length > 0) {
                editor.loadFromSVG(svgText);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error retrieving SVG data:', error);
        }
    });
}

//const clientId = `${(new Date().getTime())}-${Math.random()}`;
const postToServer = async (commandData) => {
    try {
        connectionLoadCanvas.invoke("Draw", boardId.toString(), commandData).catch(function (err) {
            return console.error(err.toString());
        });
        console.log('Posted my data', commandData);
    } catch (e) {
        console.error('Error posting command', e);
    }
};

const processUpdates = async (drawingData) => {
    debugger;
    try {
        const json = JSON.parse(drawingData);
        console.log(json);
        try {
            const command = jsdraw.SerializableCommand.deserialize(json.command, editor);
            await command.apply(editor);
            debugger;
        } catch (e) {
            console.warn('Error parsing command', e);
        }
    } catch (e) {
        console.error('Error fetching updates', e);
    }
};
async function saveNewSvg() {
    var svgData = await editor.toSVGAsync();
    var svgString = svgData.outerHTML;
    const blob = new Blob([svgString], { type: 'text/xml' });
    const formData = new FormData();
    formData.append('boardId', boardId);
    formData.append('svgBlob', blob);

    $.ajax({
        type: 'POST',
        url: '/save-svg',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            console.log('SVG sent successfully!');
        },
        error: function (error) {
            console.error('Error sending SVG blob:', error);
        }
    });
}

function download(dataurl, filename) {
    const link = document.createElement("a");
    link.href = dataurl;
    link.download = filename;
    link.click();
}