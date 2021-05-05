const splice = require("sqlite3").verbose();
let DB = new splice.Database("./DB/programChat.db", splice.OPEN_CREATE | splice.OPEN_READWRITE, (err) => {

    if (err) throw err;
    console.log("Connected to database");


    var websocket = require('ws');
    var callbackInitServer = () => {
        console.log("ThymeServer is running.");

    }

    var wss = new websocket.Server({ port: 5600 }, callbackInitServer);

    var wsList = [];
    var roomList = [];

    var roomMap = new Map();


    wss.on("connection", (ws) => {

        console.log("client connected.");
        wsList.push(ws);



        // Lobby
        ws.on("message", (data) => {

            var toJson = JSON.parse(data)
            var id;
            var password;
            var name;

            //-------------------------------------------------------------------------------------------------------------------------------Register
            if (toJson.eventName == "Register") {
                id = toJson.userID;
                password = toJson.userPassword;
                name = toJson.userName;
                DB.all("INSERT INTO UserDataProgramChat (UserID,Password,UserName) VALUES ('" + id + "','" + password + "','" + name + "')", (err, rows) => {

                    if (err) {

                        var resultData =
                        {
                            eventName: "RegisterFail",
                            userID: toJson.userID
                        }
                        //Json to string
                        jsonToStr = JSON.stringify(resultData)

                        ws.send(jsonToStr);
                        console.log(err);

                    }
                    else {

                        var resultData =
                        {
                            eventName: "RegisterSuccess",
                            userID: toJson.userID,
                            userName: toJson.userName

                        }
                        //Json to string
                        jsonToStr = JSON.stringify(resultData)

                        ws.send(jsonToStr);
                    }
                });
            }

            else if (toJson.eventName == "Login") { //-------------------------------------------------------------------------------Login
                id = toJson.userID;
                password = toJson.userPassword;
                name = toJson.userName;

                DB.all("SELECT * FROM UserDataProgramChat WHERE UserID='" + id + "' AND Password= '" + password + "'", (err, rows) => {

                    if (err) {

                        //console.log(err);
                    }

                    if (rows.length > 0) {

                        var userNameFormServer = rows[0].UserName;
                        //DB.all("SELECT UserName FROM UserDataProgramChat WHERE UserID='" + id + "'LIMIT 1", (err, rows))
                        console.log(userNameFormServer);

                        var resultData =
                        {
                            eventName: "LoginSuccess",
                            userID: toJson.userID,
                            userName: userNameFormServer

                        }
                        //Json to string
                        jsonToStr = JSON.stringify(resultData)

                        ws.send(jsonToStr);


                    }

                    else {

                        var resultData =
                        {
                            eventName: "LoginFail",
                            userID: toJson.userID
                        }
                        //Json to string
                        jsonToStr = JSON.stringify(resultData)

                        ws.send(jsonToStr);
                        //console.log(err);

                    }

                });
            }

            // -----------------------------------------------------------------------------------------CreateRoom


            else if (toJson.eventName == "CreateRoom") {

                CreateRoom(ws, toJsonObj.roomOption);

            }

            // -----------------------------------------------------------------------------------------JoinRoom
            else if (toJson.eventName == "JoinRoom") {

                JoinRoom(ws, toJsonObj.roomOption);

            }

            // -----------------------------------------------------------------------------------------LeaveRoom
            else if (toJson.eventName == "LeaveRoom") {
                LeaveRoom(ws, (status, roomKey) => {
                    let callBackMsg = {
                        eventName: "LeaveRoom",
                        status: true
                    }

                    if (status === false) {

                        callBackMsg.status = false;
                    }

                    ws.send(JSON.stringify(callBackMsg));

                    if (roomMap.get(roomKey).wsList.size <= 0) {
                        roomMap.delete(roomKey);
                    }

                });
            }

            //--------------------------------------------------------------------SentMeassge
            else if (toJson.status == "SentMessage") {

                var resultDataMessage = {

                    status: "CanSentMessage",
                    message: toJson.message,
                    roomName: toJson.roomName

                }
                jsonToStr = JSON.stringify(resultDataMessage)

                for (var i = 0; i < roomList.length; i++) {
                    for (var j = 0; j < roomList[i].wsList.length; j++) {

                        roomList[i].wsList[j].send(jsonToStr);
                    }
                }
            }

        });


        ws.on("close", () => {
            LeaveRoom(ws, (status) => {

                if (status === true) {
                    if (roomMap.get(roomKey).wsList.size <= 0) {
                        roomMap.delete(roomKey);
                    }
                }

            })
        });
    });

    function ArrayRemove(arr, value) {
        return arr.filter((element) => {
            return element != value;
        });
    }
});


function CreateRoom(ws, roomOption) {

    let roomIsFound = roomMap.has(roomOption.roomName);
    let callBackMsg = {
        eventName: "CreateRoom",
        status: false

    }

    if (roomIsFound === false) {

        callBackMsg.status = true;

        let roomName = roomOption.roomName;

        roomMap.set(roomName, {
            wsList: new Map()
        });

        roomMap.get(roomName).wsList.set(ws, {});

    }
    else {
        callBackMsg.status = false;
    }

    ws.send(JSON.stringify(callBackMsg));

}

function JoinRoom(ws, roomOption) {

    let roomName = roomOption.roomName;
    let roomIsFound = roomOption.has(roomName);

    let callBackMsg = {
        eventName: "JoinRoom",
        status: false

    }

    if (roomIsFound === true) {

        let isFoundClientInRoom = roomMap.get(roomName).wsList.has(ws);

        if (isFoundClientInRoom === true) {
            callBackMsg.status = false;
        }

        else {
            roomMap.get(roomName).wsList.set(ws, {});
            callBackMsg.status = true;
            callBackMsg.roomOption = JSON.stringify(roomList[indexRoom].roomOption);
        }

    }
    else {

        callBackMsg.status = false;
    }

    ws.send(JSON.stringify(callBackMsg));
}

function LeaveRoom(ws) {

    for (let roomKey of roomMap.keys()) {
        if (roomMap.get(roomKey).wsList.has(ws)) {
            callBack(roomMap.get(roomKey).wsList.delete(ws), roomKey);
            return;

        }
    }
    callBack(false, "");
    return;


}
